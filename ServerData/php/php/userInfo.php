<?php
	include("connect.php");
	

	$query = "select * from my_login WHERE email = '".$_GET["email"]."'";
	
	$result = mysqli_query($connection, $query);
	
		
	while ($row = $result->fetch_assoc()) {
		echo $row['gamename'].",".$row['email'].",".$row['firstname'].",".$row['lastname'].",".$row['age'].",".$row['country'].",".$row['activationcode']. $row['status'].";";		
	}	
	mysqli_close($connection);
	
?>