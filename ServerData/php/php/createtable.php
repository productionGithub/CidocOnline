<?php
include("connect.php");

$sql = "CREATE TABLE UserAccount (
id INT(11) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
gamename VARCHAR(50) NOT NULL,
email VARCHAR(50) NOT NULL,
password VARCHAR(255) NOT NULL,
activationcode VARCHAR(255) NOT NULL,
age INT(6),
firstname VARCHAR(50),
lastname VARCHAR(50),
country VARCHAR(50),
status INT(6),
user_create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
)";

if ($connection->query($sql) === TRUE) {
  echo "Table my_login created successfully";
} else {
  echo "Error creating table: " . $connection->error;
}

$connection->close();
?>