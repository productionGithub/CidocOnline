<?php
	include("connect.php");
	
	$password=md5($_POST['password']);
	
	
	$add = mysqli_query($connection, "update my_login SET password = '$password' WHERE activationcode = '".$_POST["code"]."'");
 
if ($add)
{
    echo "success";
}
else
{
    echo "Error";
}

mysqli_close($connection);
?>