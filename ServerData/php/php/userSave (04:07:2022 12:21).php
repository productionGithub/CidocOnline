<?php
include("connect.php");

header('Content-Type: application/json');

    //Check & log DB connexion
    if (!$connection->ping()) {
        printf ("Error: %s\n", $connection->error);
      }

    //DTOs
    //Data client -> server, get email
    class SignupModelUp
    {
      public $email;

      public function __construct($c) 
      {
          $this->email = $c;
      }
    }

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
$_POST = json_decode(file_get_contents("php://input"), true);


class ActivationCode
{
    public $code;   

    public function __construct($c) 
    {
        $this->code = $c;
    }
}

//Game name

if($_POST['gamename'] != null)
{
    $gamename=$_POST['gamename'];
    //echo $gamename;
}
else {
    $gamename = "TBD";
}

//email & Activation code
if($_POST['email'] != null)
{
    $email=$_POST['email'];
    $activationcode=md5($email.time());
    
    //echo $activationcode;
    //echo $email;
}
else {
    return json_encode(false);
}

//password
if($_POST['password'] != null)
{
    $password=md5($_POST['password']);
    //echo $password;
}

//status
$status=(int) 0;
//echo $status;

//age
if($_POST['age'] != null)
{
    $age=(int)$_POST['age'];
    //echo $age;
}
else {
    $age = (int) 0;
    //echo $age;
}

if($_POST['firstname'] != null)
{
    $firstname=$_POST['firstname'];
    //echo $firstname;
} else {
    $firstname="";
    //echo $firstname;
}

if($_POST['lastname'] != null)
{
    $lastname=$_POST['lastname'];
    //echo $lastname;
} else {
    $lastname="";
    //echo $lastname;
}

if($_POST['country'] != null)
{
    $country=$_POST['country'];
    //echo $country;
} else {
    $country="";
    //echo $country;
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
    `status`)
    VALUES(
        '$gamename',
        '$email',
        '$password',
        '$activationcode',
        '$age',
        '$firstname',
        '$lastname',
        '$country',
        '$status'
        )";

$add = mysqli_query($connection, $query);
 

if ($add)
{
    /*
    $a_code = new ActivationCode($activationcode);
    echo json_encode($a_code);
    */

        /*
    echo "Trying to send php email";
    echo '<br>';

    $recipients = array(
      "fx.talgorn@indytion.com",
      "oliviermarlet@gmail.com"
    );
    $email_to = implode(',', $recipients); // your email address
    $subject = "You received a message from god!";
    $txt = "fx.talgorn@indytion.com AND oliviermarlet@gmail.com should receive this email.";
    $headers = "From: noreply@ontomatchgame.fr";

    mail($email_to,$subject,$txt,$headers);
    /*
    echo "Echo email sent";
    echo '<br>';
    */

    echo "LALALA";
}

mysqli_close($connection);

?>