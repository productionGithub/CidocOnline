<?php
include("connect.php");

if(!empty($_GET['code']) && isset($_GET['code']))
{
$code=$_GET['code'];
$sql=mysqli_query($connection,"SELECT * FROM UserAccount WHERE activationcode='$code'");
$num=mysqli_fetch_array($sql);
if($num>0)
{
$st=0;
$result =mysqli_query($connection,"SELECT id FROM UserAccount WHERE activationcode='$code' and status='$st'");
$result4=mysqli_fetch_array($result);
if($result4>0)
 {
$st=1;
$result1=mysqli_query($connection,"UPDATE UserAccount SET status='$st' WHERE activationcode='$code'");
$msg="<H1>Your account is activated</H1><br>You can now login with your email and password.";
}
else
{
$msg ="Your account is already active, no need to activate again";
}
}
else
{
$msg ="Wrong activation code.";
}
echo $msg;
}
else{
    echo "code empty!";
}

mysqli_close($connection);
?>