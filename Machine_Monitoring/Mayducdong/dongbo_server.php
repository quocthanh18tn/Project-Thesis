<?php
include "../../connection.php";
?>
<?php
if (isset($_POST['IP'])&&isset($_POST['time'])) 
	{
		$IP = $_POST['IP'];
		$ID_client=explode('.',$IP)[3];
		$time_client = $_POST['time'];
		$time_compare = strtotime(date('Y-m-d H:i:s'))-strtotime($time_client);
	}
$uploaddir = realpath('./upload') . '/';
$uploadfile = $uploaddir . "$ID_client".'.sql';
// echo '<pre>';
	if (move_uploaded_file($_FILES['file_contents']['tmp_name'], $uploadfile)) 
	{
	    $nhan_file = 1;
	} 
	else 
	{
	    $nhan_file = 0;
	}
echo $nhan_file;
	// echo 'Here is some more debugging info:';
	// print_r($_FILES);
	// echo "\n<hr />\n";
	// print_r($_POST);
	// print "</pr" . "e>\n";
?>

<?php
//import database client
if($nhan_file==1)
{
	$database_client = "mayducdong".$ID_client;
	mysql_query("DROP DATABASE $database_client");
	mysql_query("CREATE DATABASE $database_client");
	$command="mysql.exe  -uroot -phainamautomation -hlocalhost $database_client<upload/$ID_client.sql";
	exec($command);
	//CODE DONG BO
	$sql_qtsx=mysql_query("SELECT * FROM $database_client.mayducdong_qtsx");
	while($qtsx = mysql_fetch_array($sql_qtsx))
	{
		$ID_lohang = $qtsx['ID_lohang'];
		$date_start_client = $qtsx['date_start'];
		$date_finish_client = $qtsx['date_finish'];
		$So_luong_thanh_client = $qtsx['So_luong_thanh'];
		$So_luong_lo_client = $qtsx['So_luong_lo'];
		$sql_qtsx_server = mysql_query("SELECT * FROM mms.mayducdong_qtsx WHERE mms.mayducdong_qtsx.ID_lohang ='$ID_lohang' AND mms.mayducdong_qtsx.date_finish IS NULL ");
		if (mysql_num_rows($sql_qtsx_server)==0)
		{
			$msnv = substr($ID_lohang,0,4);
			$L = substr($ID_lohang,4,4);
			$W = substr($ID_lohang,8,4);
			$H = substr($ID_lohang,12,4);
			$Hole = substr($ID_lohang,16,4);
			$date_start_insert = date('Y-m-d H:i:s',strtotime($date_start_client)+$time_compare);
			mysql_query("INSERT INTO mms.mayducdong_qtsx(msnv,L,W,H,Hole,ID_lohang,date_start,So_luong_thanh,So_luong_lo) VALUES ('$msnv','$L','$W','$H','$Hole','$ID_lohang','$date_start_insert','$So_luong_thanh_client','$So_luong_lo_client')");
			$date_start_server = $date_start_insert;
		}
		else
		{
			$date_start_server = mysql_fetch_array($sql_qtsx_server)['date_start'];
			mysql_query("UPDATE mms.mayducdong_qtsx SET mms.mayducdong_qtsx.So_luong_thanh='$So_luong_thanh_client',mms.mayducdong_qtsx.So_luong_lo='$So_luong_lo_client' WHERE mms.mayducdong_qtsx.ID_lohang ='$ID_lohang' AND mms.mayducdong_qtsx.date_start = '$date_start_server' ");
		}
		if ($date_finish_client!="")
		{
			$date_finish_insert = date('Y-m-d H:i:s',strtotime($date_finish_client)+$time_compare);
			mysql_query("UPDATE mms.mayducdong_qtsx SET mms.mayducdong_qtsx.date_finish ='$date_finish_insert' WHERE mms.mayducdong_qtsx.ID_lohang ='$ID_lohang' AND mms.mayducdong_qtsx.date_start = '$date_start_server' ");
		}
		$sql_tamdung_client = mysql_query("SELECT * FROM $database_client.mayducdong_tamdung WHERE $database_client.mayducdong_tamdung.date_start ='$date_start_client' and $database_client.mayducdong_tamdung.ID_lohang ='$ID_lohang'");
		while ($tamdung_client = mysql_fetch_array($sql_tamdung_client))
		{
			$date_pause_client = $tamdung_client['date_pause'];
			$date_resume_client = $tamdung_client['date_resume']; 
			$trangthai_pause_client = $tamdung_client['Trang_thai_start'];
			$trangthai_resume_client = $tamdung_client['Trang_thai_stop'];
			if ($trangthai_pause == 0)
			{
					$date_pause_insert = date('Y-m-d H:i:s',strtotime($date_pause_client)+$time_compare);
					mysql_query("INSERT INTO mms.mayducdong_tamdung(ID_lohang,date_pause,date_start) VALUES ('$ID_lohang','$date_pause_insert','$date_start_server') ");
			}
			if ($trangthai_resume == 0)
			{
					$date_resume_insert = date('Y-m-d H:i:s',strtotime($date_resume_client)+$time_compare);
					mysql_query("UPDATE mms.mayducdong_tamdung SET mms.mayducdong_tamdung.date_resume ='$date_resume_insert' WHERE mms.mayducdong_tamdung.ID_lohang ='$ID_lohang' AND mms.mayducdong_tamdung.date_start = '$date_start_server' AND mms.mayducdong_tamdung.date_resume IS NULL ");
			}

		} 
		$sql_tangca_client = mysql_query("SELECT * FROM $database_client.mayducdong_tangca WHERE $database_client.mayducdong_tangca.date_start ='$date_start_client' and $database_client.mayducdong_tangca.ID_lohang ='$ID_lohang'");
		while ($tangca_client = mysql_fetch_array($sql_tangca_client))
		{
			$date_start_overtime_client = $tangca_client['date_start_overtime'];
			$date_finish_overtime_client = $tangca_client['date_finish_overtime']; 
			$trangthai_start_overtime_client = $tangca_client['Trang_thai_start'];
			$trangthai_finish_overtime_client = $tangca_client['Trang_thai_stop'];
			$date_start_overtime_insert = date('Y-m-d H:i:s',strtotime($date_start_overtime_client)+$time_compare);
			$day_overtime_server =date('Y-m-d',$date_start_overtime_insert);
			if ($trangthai_start_overtime_client == 0)
			{
					mysql_query("INSERT INTO mms.mayducdong_tangca(ID_lohang,date_start_overtime,date_start) VALUES ('$ID_lohang','$date_start_overtime_insert','$date_start_server') ");
			}
			if ($trangthai_finish_overtime_client == 0)
			{
					$date_finish_overtime_insert = date('Y-m-d H:i:s',strtotime($date_finish_overtime_client)+$time_compare);
					mysql_query("UPDATE mms.mayducdong_tangca SET mms.mayducdong_tangca.date_finish_overtime ='$date_finish_overtime_insert' WHERE mms.mayducdong_tangca.ID_lohang ='$ID_lohang' AND mms.mayducdong_tangca.date_start = '$date_start_server' AND mms.mayducdong_tangca.date_finish_overtime IS NULL AND mms.mayducdong_tangca.date_start_overtime LIKE '$day_overtime_server%'");
			}

		} 
	}

}
?>
<?php
// // your config
// $filename = realpath('./upload').'/backup.sql';
// $dbHost = 'localhost';
// $dbUser = 'root';
// $dbPass = 'hainamautomation';
// $dbName = 'backup';
// $maxRuntime = 8; // less then your max script execution limit
// $deadline = time()+$maxRuntime; 
// $progressFilename = $filename.'_filepointer'; // tmp file for progress
// $errorFilename = $filename.'_error'; // tmp file for erro

// mysql_connect($dbHost, $dbUser, $dbPass) OR die('connecting to host: '.$dbHost.' failed: '.mysql_error());
// mysql_query("DROP DATABASE backup");
// mysql_query("CREATE DATABASE IF NOT EXISTS backup");
// mysql_select_db($dbName) OR die('select db: '.$dbName.' failed: '.mysql_error());

// ($fp = fopen($filename, 'w+')) OR die('failed to open file:'.$filename);

// // check for previous error
// if( file_exists($errorFilename) ){
//     die('<pre> previous error: '.file_get_contents($errorFilename));
// }

// // activate automatic reload in browser
// echo '<html><head> <meta http-equiv="refresh" content="'.($maxRuntime+2).'"><pre>';

// // go to previous file position
// $filePosition = 0;
// if( file_exists($progressFilename) ){
//     $filePosition = file_get_contents($progressFilename);
//     fseek($fp, $filePosition);
// }

// $queryCount = 0;
// $query = '';
// while( $deadline>time() AND ($line=fgets($fp, 1024000)) ){
//     if(substr($line,0,2)=='--' OR trim($line)=='' ){
//         continue;
//     }

//     $query .= $line;
//     if( substr(trim($query),-1)==';' ){
//         if( !mysql_query($query) ){
//             $error = 'Error performing query \'<strong>' . $query . '\': ' . mysql_error();
//             file_put_contents($errorFilename, $error."\n");
//             exit;
//         }
//         $query = '';
//         file_put_contents($progressFilename, ftell($fp)); // save the current file position for 
//         $queryCount++;
//     }
// }

// if( feof($fp) ){
//     echo 'dump successfully restored!';
// }else{
//     echo ftell($fp).'/'.filesize($filename).' '.(round(ftell($fp)/filesize($filename), 2)*100).'%'."\n";
//     echo $queryCount.' queries processed! please reload or wait for automatic browser refresh!';
// }
?>
