<?php
include("connect.php");
 
mysqli_query($connection, "DELETE FROM my_login WHERE email = '".$_GET["email"]."');
mysqli_close($connection);
?>

