<?php

header("Access-Control-Allow-Headers: Origin, X-Requested-With, Content-Type, Accept, Cache-Control");
header('Access-Control-Allow-Methods: GET, POST, PUT');
header('content-type: application/json; charset=utf-8');


//echo $_SERVER['HOME'];
//upload_max_filesize = 25M;
error_reporting(0);


date_default_timezone_set('Singapore');
//printf($_FILES);
$WebURL ="D:\\VIVEK\\Logistics_Publish\\TempAttachments\\";
$SAPURL = "";
$eMSG = "";
//print_r($_POST);
$path_parts = pathinfo($_FILES["file"]["name"]);
$fileExt = $path_parts['extension'];
$fileNewName = $path_parts['filename'].'_'.time();
$filePath = $fileNewName.'.'.$path_parts['extension'];
		$fileName = $_FILES["file"]["name"];				
		$target_file = $WebURL . basename($filePath);
		
if($_FILES["file"]['size'] > 100000000)
{
	$eMSG = "File size should be less than 100mb";
	$status = 'false';
}
else
{
	if(move_uploaded_file($_FILES["file"]["tmp_name"], $target_file ))
	{
		$eMSG = "";
		$status = 'true';
	}
	else
	{
		$eMSG = "something went wrong.";
		$status = 'false';
	}
		
}

	$rarr = array('FileName'=>$fileNewName,'FilePath'=>$WebURL,'FileExten'=>$fileExt,'Status'=>$status,'errMSG'=>$eMSG);
		echo json_encode($rarr);
			
//print_r($arr);
?>
