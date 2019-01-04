<?php
include "../../connection.php";
?>
<?php
	
	
?>
<?php
//lay du lieu tu may duc dong gui len
$msnv = $_POST['ID'];
$mode = $_POST['Mode'];
$L = $_POST['L'];
$W = $_POST['W'];
$H = $_POST['H'];
$Hole = $_POST['Hole'];
$ButtonState = $_POST['ButtonState'];
//
$CountBar = $_POST['CountBar'];
$CountHole = $_POST['CountHole'];
//
$msnv =substr(number_format($msnv/10000,4),2);//lay 4 ki tu
$L =substr(number_format($L/1000,4),2);//lay 4 ki tu
$W =substr(number_format($W/1000,4),2);//lay 4 ki tu
$H =substr(number_format($H/1000,4),2);//lay 4 ki tu
$Hole =substr(number_format($Hole/1000,4),2);//lay 4 ki tu
$ID_lohang = $msnv.$L.$W.$H.$Hole;
?>

<?php
//////////////////////////////////// XU LY CAC NU NHAN //////////////////////////////////////////////////////////
$thanhcong=0;
//NÚT NHẤN START
if ($ButtonState=="Start")
{
	$sql = mysql_query("SELECT ID_lohang,date_finish FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_finish IS NULL");
	$num = mysql_num_rows($sql);
	if ($num==0)
	{
		$datetime = date('Y-m-d H:i:s');
		mysql_query("INSERT INTO mayducdong_qtsx(msnv,L,W,H,Hole,ID_lohang,date_start) VALUES ('$msnv','$L','$W','$H','$Hole','$ID_lohang','$datetime') ");
		if(mysql_num_rows(mysql_query("SELECT ID_lohang,date_start FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_start='$datetime'"))==1) {echo 1;$thanhcong=1;$date_start=$datetime;}//thành công
		else echo 13;//lỗi
	}
	else 
	{
		$sql = mysql_query("SELECT ID_lohang,date_resume FROM mayducdong_tamdung WHERE ID_lohang='$ID_lohang' AND date_resume IS NULL");
		$num = mysql_num_rows($sql);//kiểm tra xem có đang pause ko(sua lai them cot date_start vao bang pause)
		if($num>0) echo 2;// đang Pause
		else echo 3;//đã start rồi
	}
}
//NHẤN NÚT FINISH
elseif ($ButtonState=="Finish") 
{
	$sql = mysql_query("SELECT ID_lohang,date_start,date_finish,So_luong_lo,So_luong_thanh FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_finish IS NULL");
	$num = mysql_num_rows($sql);
	if ($num==1)
	{

		$sql1 = mysql_query("SELECT ID_lohang,date_resume FROM mayducdong_tamdung WHERE ID_lohang='$ID_lohang' AND date_resume IS NULL");
		$num1= mysql_num_rows($sql1);//kiểm tra xem có đang pause ko(sua lai them cot date_start vao bang pause)
		if($num1>0) echo 2;// đang Pause
		else
		{
		$datetime = date('Y-m-d H:i:s');
		//update so luong
		$dulieu= mysql_fetch_array($sql);
		$date_start = $dulieu['date_start'];
		$So_luong_thanh = $dulieu['So_luong_thanh'] + $CountBar;
		$So_luong_lo = $dulieu['So_luong_lo']+ $CountHole;
		//
		mysql_query("UPDATE mayducdong_qtsx SET date_finish ='$datetime',So_luong_lo='$So_luong_lo',So_luong_thanh='$So_luong_thanh' WHERE ID_lohang='$ID_lohang' AND date_start ='$date_start'");
		if(mysql_num_rows(mysql_query("SELECT ID_lohang,date_finish FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_finish='$datetime'"))==1) {echo 1;$thanhcong=1;}//thành công
		else echo 13;//lỗi
		}
	}
	else 
	{
		echo 4;
	}

}
//NHẤN NÚT PAUSE
elseif ($ButtonState=="Pause") 
{
	$sql = mysql_query("SELECT ID_lohang,date_start,date_finish,So_luong_thanh,So_luong_lo FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_finish IS NULL");
	$num = mysql_num_rows($sql);
	if ($num==1)
	{
		$sql1 = mysql_query("SELECT ID_lohang,date_resume FROM mayducdong_tamdung WHERE ID_lohang='$ID_lohang' AND date_resume IS NULL");
		$num1= mysql_num_rows($sql1);//kiểm tra xem có đang pause ko(sua lai them cot date_start vao bang pause)
		if($num1>0) echo 2;// đang Pause
		else
		{
		$datetime = date('Y-m-d H:i:s');
		$dulieu= mysql_fetch_array($sql);
		//update so luong
		$date_start = $dulieu['date_start'];
		$So_luong_thanh = $dulieu['So_luong_thanh'] + $CountBar;
		$So_luong_lo = $dulieu['So_luong_lo']+ $CountHole;
		mysql_query("UPDATE mayducdong_qtsx SET So_luong_lo='$So_luong_lo',So_luong_thanh='$So_luong_thanh' WHERE ID_lohang='$ID_lohang' AND date_start ='$date_start'");
		//
		mysql_query("INSERT mayducdong_tamdung (ID_lohang,date_pause,date_start) VALUES ('$ID_lohang','$datetime
			','$date_start')");
		if(mysql_num_rows(mysql_query("SELECT ID_lohang,date_pause FROM mayducdong_tamdung WHERE ID_lohang='$ID_lohang' AND date_pause='$datetime'"))==1) {echo 1;$thanhcong=1;}//thành công
		else echo 13;//lỗi
		}
	}
	else 
	{
		echo 4;
	}

}
//NHẤM NÚT RESUME
elseif ($ButtonState=="Resume")
{
	$sql = mysql_query("SELECT ID_lohang,date_finish FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_finish IS NULL");
	$num = mysql_num_rows($sql);
	if ($num==0)
	{
		echo 4;
	}
	else 
	{
		
		$sql = mysql_query("SELECT ID_lohang,date_resume,date_pause,date_start FROM mayducdong_tamdung WHERE ID_lohang='$ID_lohang' AND date_resume IS NULL");
		$num = mysql_num_rows($sql);//kiểm tra xem có đang pause ko(sua lai them cot date_start vao bang pause)
		if($num>0)// đang Pause
		{
			$datetime = date('Y-m-d H:i:s');
			$array=mysql_fetch_array($sql);
			$date_pause=$array['date_pause'];
			$date_start=$array['date_start'];

			mysql_query("UPDATE mayducdong_tamdung SET date_resume ='$datetime' WHERE ID_lohang='$ID_lohang' AND date_pause ='$date_pause'");
		if(mysql_num_rows(mysql_query("SELECT ID_lohang,date_resume FROM mayducdong_tamdung WHERE ID_lohang='$ID_lohang' AND date_resume='$datetime'"))==1) {echo 1;$thanhcong=1;}//thành công
		else echo 13;//lỗi
		}
		else echo 5;//đã start rồi
		
	}
}
elseif ($ButtonState=="Count")
{
		$sql = mysql_query("SELECT ID_lohang,date_start,date_finish,So_luong_thanh,So_luong_lo FROM mayducdong_qtsx WHERE ID_lohang='$ID_lohang' AND date_finish IS NULL");
		$datetime = date('Y-m-d H:i:s');
		$dulieu= mysql_fetch_array($sql);
		//update so luong
		$date_start = $dulieu['date_start'];
		$So_luong_thanh = $dulieu['So_luong_thanh'] + $CountBar;
		$So_luong_lo = $dulieu['So_luong_lo']+ $CountHole;
		mysql_query("UPDATE mayducdong_qtsx SET So_luong_lo='$So_luong_lo',So_luong_thanh='$So_luong_thanh' WHERE ID_lohang='$ID_lohang' AND date_start ='$date_start'");
		//
		echo 1;
}

?>

<?php
////////////////////////////////////KIỂM TRA GIỜ TĂNG CA//////////////////////////////////////////////////
if ($thanhcong == 1)
{
	$day=date('Y-m-d');
	$datetime=date('Y-m-d H:i:s');
	$num_holiday =mysql_num_rows(mysql_query("SELECT * FROM holiday WHERE holiday_date = '$day'"));
	$sunday = strtotime($day);
	$sunday = getdate($sunday)['weekday'];
	if ($sunday=='Sunday'||$num_holiday >0||((strtotime($datetime)-strtotime("$day 17:15:00")>0))) 
	{
		if ($ButtonState=='Start'||$ButtonState=='Resume')
		{
			mysql_query("INSERT INTO mayducdong_tangca (ID_lohang,date_start_overtime,date_start) VALUES ('$ID_lohang','$datetime','$date_start') ");
		}
		elseif ($ButtonState=='Finish'||$ButtonState=='Pause')
		{
			$sql = mysql_query("SELECT * FROM mayducdong_tangca WHERE ID_lohang='$ID_lohang' AND date_start ='$date_start' AND date_start_overtime LIKE '$day%' AND date_finish_overtime IS NULL ");
			if(mysql_num_rows($sql)==1)
			{
				mysql_query("UPDATE mayducdong_tangca SET date_finish_overtime ='$datetime' WHERE ID_lohang='$ID_lohang' AND date_start ='$date_start' AND date_start_overtime LIKE '$day%' AND date_finish_overtime IS NULL");
			}
		}

	}
}
?>
