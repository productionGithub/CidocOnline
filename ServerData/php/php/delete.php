<?php
include("connect.php");
 
mysqli_query($connection, "DELETE FROM my_login");
mysqli_close($connection);
?>