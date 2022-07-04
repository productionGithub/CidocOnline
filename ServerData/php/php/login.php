<?php
	include("connect.php");
	

	$query = "select * from my_login WHERE email = '".$_GET["email"]."'"." AND password ='".md5($_GET["password"])."'";
	
	$result = mysqli_query($connection, $query);
	
		
	while ($row = $result->fetch_assoc()) {
		echo $row['status'];		
	}	
	mysqli_close($connection);	
?>
