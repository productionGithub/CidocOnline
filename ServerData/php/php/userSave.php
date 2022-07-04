<?php
    include("connect.php");

    header('Content-Type: application/json');

    //Check & log DB connexion
    if (!$connection->ping()) {
      printf ("Error: %s\n", $connection->error);
    }

    //DTOs
    //Data server -> client, create activation code from email
    class SignupModelDown
    {
      public $code;   

      public function __construct($c) 
      {
          $this->code = $c;
      }
    }
    //End DTOs

    $body = file_get_contents('php://input');//Needed because data are json, not from xxx-form-encoded
    $parsed = json_decode($body);


    //gamename field
    if(isset($parsed->gamename) && $parsed->gamename != '')
    {
        $gamename = $parsed->gamename;
    }
    else
    {
        $gamename = '';
    }


    //email field
    if(isset($parsed->email) && $parsed->email != '')
    {
        $email = $parsed->email;
        $activationcode =  hash("sha256", $email, $binary=false);
    }
    else
    {
        error_log("[OntoMatchGame] Email field empty. Can't create account.", 0);
        ReturnEmptyString();
        exit("[OntoMatchGame] Email field empty. Can't create account.");
    }

    //password field
    if(isset($parsed->password) && $parsed->password != '')
    {
        $password = hash("sha256", $parsed->password, $binary=false);
    }
    else
    {
        error_log("[OntoMatchGame] Password field empty. Can't create account.", 0);
        ReturnEmptyString();
        exit("[OntoMatchGame] Password field empty. Can't create account.");
    }

    //status code, 0 at creation time
    $status = 0;

    //age field
    if(isset($parsed->age) && $parsed->age != '')
    {
        $age =  $parsed->age;
    }
    else
    {
        $age=0;
    }

    //firstname field
    if(isset($parsed->firstname) && $parsed->firstname != '')
    {
        $firstname =  $parsed->firstname;
    }
    else
    {
        $firstname='';
    }

    //lastname field
    if(isset($parsed->lastname) && $parsed->lastname != '')
    {
        $lastname =  $parsed->lastname;
    }
    else
    {
        $lastname='';
    }

    //country field
    if(isset($parsed->country) && $parsed->country != '')
    {
        $country =  $parsed->country;
    }
    else
    {
        error_log("[OntoMatchGame] Country field empty. Can't create account.", 0);
        ReturnEmptyString();
        exit("[OntoMatchGame] Country field empty. Can't create account.");
    }

    //optin field
    if(isset($parsed->optin) && $parsed->optin == 'true') //optin is necessarily set (false by default)
    {
        //echo $parsed->optin;
        $optin =  1;

    }
    else
    {
        //echo $parsed->optin;
        $optin =  0;
    }

    //Query
    $query = "INSERT INTO ontomatchgame.UserAccount(
        `gamename`,
        `email`,
        `password`,
        `activationcode`,
        `age`,
        `firstname`,
        `lastname`,
        `country`,
        `status`,
        `optin`
        )
        VALUES(
            '$gamename',
            '$email',
            '$password',
            '$activationcode',
            '$age',
            '$firstname',
            '$lastname',
            '$country',
            '$status',
            '$optin'
            )";

    $add = mysqli_query($connection, $query);

    if ($add)
    {
        //Encapsulate as JSON object
        $instance = new SignupModelDown($activationcode);
        echo json_encode($instance);

        //Send email with activation code
        //TODO : For production, add "oliviermarlet@gmail.com"
        $recipients = array(
            $email,
            "fx.talgorn@indytion.com"
            );
            
        $email_to = implode(',', $recipients); // your email address
        $subject = "Activate your OntoMatchGame account!";
        $txt = "Please, click on the link below to activate your OntoMatchGameAccount:\nhttps://ontomatchgame.huma-num.fr/php/verification.php?code=".$activationcode;
        $headers = "From: noreply@ontomatchgame.fr";

        mail($email_to,$subject,$txt,$headers);
    }
    else
    {
        echo $sqli->error;
        error_log("[OntoMatchGame] Sorry. Database server error. Can't create account. See DB administrator for details.", 0);
        ReturnEmptyString();
        exit("[OntoMatchGame] Sorry. Database server error. Can't create account. See DB administrator for details.");
    }

    function ReturnEmptyString() {
        $instance = new SignupModelDown("");
        echo json_encode($instance);
    }

    mysqli_close($connection);
?>
