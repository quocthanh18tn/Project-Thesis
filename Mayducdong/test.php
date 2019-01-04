<!DOCTYPE HTML>
<html>
<head>
	<meta name="viewport" content="width=device-width, initial-scale=1">
  	<script src="../../Library/sweetalert.min.js"></script>
  	<script src="../../Library/jquery-3.1.1.min.js" type="text/javascript"></script>
	<script>
		$(document).ready(function(){
		    $("#xacnhan").click(function(){
		     	var msnv = $("#ID").val();
				var mode = $("#Mode").val();
				var L = $("#L").val();
				var W = $("#W").val();
				var H = $("#H").val();
				var Hole = $("#Hole").val();
				var CountHole = $("#CountHole").val();
				var CountBar = $("#CountBar").val();
				var ButtonState = $("#ButtonState").val();
				$.ajax({
				url: 'mayducdong.php',
				dataType: 'text',
				type: 'POST',
				data: {ID:msnv,Mode:mode,L:L,W:W,H:H,Hole:Hole,CountHole:CountHole,CountBar:CountBar, ButtonState:ButtonState},
				success: function(data){
							$("#info").html(data);
						}
					}); // close ajax
		    });
		});
	</script>
</head>
<body>
	<p>ID</p>
	<input type="text" id="ID"><br>
	<p>Mode</p>
	<input type="text" id="Mode"><br>
	<p>L</p>
	<input type="text" id="L"><br>
	<p>W</p>
	<input type="text" id="W"><br>
	<p>H</p>
	<input type="text" id="H"><br>
	<p>Hole</p>
	<input type="text" id="Hole"><br>
	<p>CountHole</p>
	<input type="text" id="CountHole"><br>
	<p>CountBar</p>
	<input type="text" id="CountBar"><br>
	<p>ButtonState</p>
	<input type="text" id="ButtonState"><br>
	<input type="button" id="xacnhan" value="XÁC NHẬN">	
	<p>	/// 
        /// fucntion send to server
        /// return code :
        ///     0 rot mang
        ///     1 send ok
        ///     2 dang pause 
        ///     3 đã start rồi
        ///     4 chưa start
        ///     5 chưa pause
        /// 	13 lỗi
        ///</p>
    <p>DỮ LIỆU TRẢ VỀ</p>
    <div id="info"></div>
</body>
</html>

