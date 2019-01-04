<?php
include 'connection.php';
//xoa cac trang thai neu lo hang da finish
$sql=mysql_query("SELECT * FROM mayducdong_qtsx where date_finish IS NOT NULL");
if (mysql_num_rows($sql)!=0)
	{
	while($array=mysql_fetch_array($sql))
	{
		$ID_lohang=$array['ID_lohang'];
		$date_start=$array['date_start'];
		$State=$array['Trang_thai_stop'];
		$sql_tamdung=mysql_query("SELECT * FROM mayducdong_tamdung where date_start='$date_start' and ID_lohang='$ID_lohang' ");
		if (mysql_num_rows($sql_tamdung)!=0)
			{
				while($array_tamdung=mysql_fetch_array($sql_tamdung))
				{
					if($array_tamdung['Trang_thai_start']=='1'&&$array_tamdung['Trang_thai_stop']=='1')
						$flag_check_tamdung=1;
					else $flag_check_tamdung=0;
				}
			}
		else $flag_check_tamdung=1;

		$sql_tangca=mysql_query("SELECT * FROM mayducdong_tangca where date_start='$date_start' and ID_lohang='$ID_lohang' ");
		if (mysql_num_rows($sql_tangca)!=0)
			{
				while($array_tangca=mysql_fetch_array($sql_tangca))
				{
					if($array_tangca['Trang_thai_start']=='1'&&$array_tangca['Trang_thai_stop']=='1')
						$flag_check_tangca=1;
					else $flag_check_tangca=0;
				}
			}
		else $flag_check_tangca=1;
		if($flag_check_tangca ==1 && $flag_check_tamdung==1&&$State==1)
			{
				mysql_query("DELETE FROM mayducdong_tamdung where ID_lohang='$ID_lohang' and date_start='$date_start'");
				mysql_query("DELETE FROM mayducdong_tangca where ID_lohang='$ID_lohang' and date_start='$date_start'");
				mysql_query("DELETE FROM mayducdong_qtsx where ID_lohang='$ID_lohang' and date_start='$date_start'");
			}
		}

	}



$datetime = date('Y-m-d H:i:s');
$IpServer="http://192.168.137.1";
$IP=$_POST['IP'];
// $IP="192.168.114.115";
//This needs to be the full path to the file you want to send.
//dump database ra ổ D:\dump.sql -> copy vao cung folder voi file local extension form.sql -> sau đó send qua server
$command="mysqldump.exe -e -uroot -phainamautomation -hlocalhost mmslocalmayducdong>D:\dump.sql";
exec($command);
copy('D:\dump.sql','C:\Program Files (x86)\Ampps\www\Mayducdong\dump.sql');
// rename url ip address of server
$target_url = $IpServer.'/Machine_Monitoring/Mayducdong/dongbo_server.php';
$file_name_with_full_path = realpath('./dump.sql');
/* curl will accept an array here too.
 * Many examples I found showed a url-encoded string instead.
 * Take note that the 'key' in the array will be the key that shows up in the
 * $_FILES array of the accept script. and the at sign '@' is required before the
 * file name.
 */

$post = array('IP' => "$IP",'time'=>"$datetime",'file_contents'=>'@'.$file_name_with_full_path);

$ch = curl_init();
curl_setopt($ch, CURLOPT_SAFE_UPLOAD, false);
curl_setopt($ch, CURLOPT_URL,$target_url);
curl_setopt($ch, CURLOPT_POST,1);
curl_setopt($ch, CURLOPT_POSTFIELDS, $post);
$result=curl_exec ($ch);
curl_close ($ch);
if ($result ==1)
{
	//update tat ca cac field lên 1
	$sql_tamdung=mysql_query("SELECT * FROM mayducdong_tamdung");
	if(mysql_num_rows($sql_tamdung)!=0)
	{
		while($array_tamdung=mysql_fetch_array($sql_tamdung))
		{
			$ID_lohang=$array_tamdung['ID_lohang'];
			$date_start=$array_tamdung['date_start'];
			$Trang_thai_start=$array_tamdung['Trang_thai_start'];
			$Trang_thai_stop=$array_tamdung['Trang_thai_stop'];
			if ($Trang_thai_start ==0)
				mysql_query("UPDATE mayducdong_tamdung SET Trang_thai_start='1' where ID_lohang='$ID_lohang' and date_start='$date_start' ");
			if ($Trang_thai_stop ==0)
				mysql_query("UPDATE mayducdong_tamdung SET Trang_thai_stop='1' where ID_lohang='$ID_lohang' and date_start='$date_start' ");
		}
	}
	$sql_tangca=mysql_query("SELECT * FROM mayducdong_tangca");
	if(mysql_num_rows($sql_tangca)!=0)
	{
		while($array_tangca=mysql_fetch_array($sql_tangca))
		{
			$ID_lohang=$array_tangca['ID_lohang'];
			$date_start=$array_tangca['date_start'];
			$Trang_thai_start=$array_tangca['Trang_thai_start'];
			$Trang_thai_stop=$array_tangca['Trang_thai_stop'];
			if ($Trang_thai_start ==0)
				mysql_query("UPDATE mayducdong_tangca SET Trang_thai_start='1' where ID_lohang='$ID_lohang' and date_start='$date_start' ");
			if ($Trang_thai_stop ==0)
				mysql_query("UPDATE mayducdong_tangca SET Trang_thai_stop='1' where ID_lohang='$ID_lohang' and date_start='$date_start' ");
		}
	}
	$sql_qtsx=mysql_query("SELECT * FROM mayducdong_qtsx");
	if(mysql_num_rows($sql_qtsx)!=0)
	{
		while($array_qtsx=mysql_fetch_array($sql_qtsx))
		{
			$ID_lohang=$array_qtsx['ID_lohang'];
			$date_start=$array_qtsx['date_start'];
			$Trang_thai_stop=$array_qtsx['Trang_thai_stop'];
			if ($Trang_thai_stop ==0)
				mysql_query("UPDATE mayducdong_qtsx SET Trang_thai_stop='1' where ID_lohang='$ID_lohang' and date_start='$date_start' ");
		}
	}


}
// mysql_query("DROP DATABASE IF NOT EXISTS nhaan");
// mysql_query("CREATE DATABASE IF NOT EXISTS nhaan");
// $command="mysql.exe  -uroot -phainamautomation -hlocalhost nhaan<form.sql";
// exec($command);
?>
