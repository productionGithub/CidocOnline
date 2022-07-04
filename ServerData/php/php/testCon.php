<?php
	$host = "mysql80a.db.huma-num.fr";
	$dbname = "ontomatchgame";
	$dbusername = "ontomatchgame";
	$dbpassword = "GgNts2To_g9UVDWrGpp";
	
	
	$connexion = mysql_connect($host, $dbusername, $dbpassword, $dbname);

	if ($connexion->ping()) {
  		printf ("Our connection is ok!\n"); 
		} else {
		  printf ("Error: %s\n", $mysqli->error); 
		}

?>
