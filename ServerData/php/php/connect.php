<?php

	$connection = mysqli_init();

	if (!$connection) {
		echo "false";
	}
	

	mysqli_ssl_set($connection, NULL,NULL,'/shared/hncert/__db_huma-num_fr_interm_root.cer', '/dev/null', NULL);
	mysqli_real_connect($connection, "mysql80a.db.huma-num.fr", "ontomatchgame", "GgNts2To_g9UVDWrGpp", "ontomatchgame");

	/*
	echo "Initiate DB connection";

	$connection = mysqli_init();
	// Do not check SSL certificate
	$connection -> options(MYSQLI_OPT_SSL_VERIFY_SERVER_CERT, false);

	$host = "mysql80a.db.huma-num.fr";
	$dbname = "ontomatchgame";
	$dbusername = "ontomatchgame";
	$dbpassword = "GgNts2To_g9UVDWrGpp";
	
	echo "Trying to connect to DB...";

	$connection -> real_connect($host, $dbusername, $dbpassword, $dbname);

	echo $connection;
	*/
?>	