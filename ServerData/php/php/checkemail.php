<?php

    include("connect.php");

    if (!$connection->ping()) {
      printf ("Error: %s\n", $connection->error);
    }

    //DTO down, bool result
    class EmailCheck
    {
      public $doesExist;

      public function __construct($b) 
      {
          $this->doesExist = $b;
      }
    }

    //Search for email in DB.UserAccount
    $query = "SELECT * FROM UserAccount WHERE email='".$_GET["email"]."'";    
    $result = $connection->query($query);

    if ($result->num_rows > 0) {//Email found
          $result = new EmailCheck(true);
          echo json_encode($result);
      } else {
        $result = new EmailCheck(false);
        echo json_encode($result);
    } 

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
    
    mysqli_close($connection);

?>