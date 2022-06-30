<?php

    include("connect.php");
    
    class ActivationCode
    {
      public $code;   

      public function __construct($c) 
      {
          $this->code = $c;
      }
    }

    $code = new ActivationCode("code29");

    echo $code;
    /*
    echo '<br>';
    echo "After include...";
    echo '<br>';
    */

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