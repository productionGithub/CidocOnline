<form name="reset" action="updatepassword.php" method="post">
<table width="100%"  border="0">
<tr>
 <th height="62" scope="row">Code </th>
 <td width="71%"><input type="text" name="code" id="code" value="<?php echo $_GET["code"];?>"  class="form-control" required /></td>
</tr>

<tr>
 <th height="62" scope="row">New Password </th>
 <td width="71%"><input type="password" name="password" id="password" value=""  class="form-control" required /></td>
</tr>
<tr>
<th height="62" scope="row"></th>
<td width="71%"><input type="submit" name="login" value="Submit" class="btn-group-sm" /> </td>
</tr>
</table>
</form>