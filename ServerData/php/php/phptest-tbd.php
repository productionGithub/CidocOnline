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

    $body = file_get_contents('php://input');//Needed because data are json, not from xxx-form-encoded
    $parsed = json_decode($body);
    $email = $parsed->email;

    echo $email;
    
    //Generate activation code
    $email =  hash("sha256", $parsed->email, $binary=false);

    //Encapsulate as JSON object
    $instance = new SignupModelDown($email);
    echo json_encode($instance);


    // $query = "SELECT * FROM User WHERE email='".$_GET["email"]."'";

    /*
    echo $query;
    echo '<br>';
    */
    //$result = $connection->query($query);

    /*
    echo "Before result..";
    echo '<br>';
    */

    /*
    if ($result->num_rows > 0) {
        // output data of each row
        while($row = $result->fetch_assoc()) {
          echo "true";//$row["email"];

          echo "username: " . $row["username"]. " : " . "username: " . $row["email"];
          echo '<br>';

        }
      } else {
        echo "false";
    } 
    */

    /*
    echo "Trying to send php email";
    echo '<br>';
    */

    /*
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

    mysqli_close($connection);

?>