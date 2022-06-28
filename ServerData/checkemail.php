<?php
    include("connect.php");
    
    $query = "select * from my_login WHERE email = '".$_GET["email"]."'";
    
    $result = mysqli_query($connection, $query);
    
    if($result)
    {
    while ($row = $result->fetch_assoc()) {
        echo $row['email'];      
    }
    else
    {
        echo "false";
    }
    mysqli_close($connection);
    
?>