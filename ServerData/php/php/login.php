<?php
	include("connect.php");
	
	header('Content-Type: application/json');

	//Check & log DB connexion
	if (!$connection->ping()) {
		printf ("Error: %s\n", $connection->error);
	}

	//DTO down, bool result
	class SigninModelDown
	{
		public $login;

		public function __construct($b) 
		{
			$this->login = $b;
		}
	}


	$body = file_get_contents('php://input');//Needed because data are json, not from xxx-form-encoded
    $parsed = json_decode($body);
	//echo $parsed->email;
	//echo $parsed->password;


	//email field
	if(isset($parsed->email) && $parsed->email != '')
	{
		$email = $parsed->email;
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


	//Search for email / password in DB.UserAccount
	$query = "SELECT * from ontomatchgame.UserAccount WHERE email = '".$email."'"." AND password ='".$password."'"; 
	//echo $query;
	
	$result = $connection->query($query);
	//var_dump($result);

	if ($result->num_rows > 0)
	{//Email found
		$result = new SigninModelDown(true);
		echo json_encode($result);
	}
	else
	{
	  $result = new SigninModelDown(false);
	  echo json_encode($result);
  	} 

	mysqli_close($connection);	
?>
