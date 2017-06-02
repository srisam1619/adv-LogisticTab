App.service('util_SERVICE', ['$http', '$window', '$cookieStore', '$rootScope', function ($http, $window, $cookie, $rootScope) {
    var urlsd = window.location.href.split("/");
    //this.url = "http://192.168.0.38:85/Logistics.asmx/";
	//this.Host = "http://192.168.0.38:85/";
	this.url = "http://192.168.0.38:86/Logistics.asmx/";
	this.Host = "http://192.168.0.38:86/";  
	//this.Host = "http://localhost:8012/do/";
	
    this.config = {
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
        }
    }
	
	this.configgoogle = {
        headers: {
            'Content-Type': 'application/json charset=utf-8;',
			'Access-Control-Allow-Origin': '*',
			'Access-Control-Allow-Methods':'GET,PUT,POST,DELETE,OPTIONS',
			
        }
    }
	
	this.islogin = function()
	{
		if($cookie.get('Islogin')==false || $cookie.get('Islogin')===undefined)
		{
			window.location = "login.html";
		}
	}
	
	//GetDriverNames
    this.GetDriverNames = function (company) {
        var promise = $http.post(this.url + "GetDriverNames", "sCompany="+company, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	
	//GetDriverList
    this.GetDriverList = function (company) {
        var promise = $http.post(this.url + "GetDriverList", "sCompany="+company, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetDriverList
    this.InsertUpdateDeleteDriver = function (data,company) {
        var promise = $http.post(this.url + "InsertUpdateDeleteDriver", "sCompany="+company+"&sJsonInput="+JSON.stringify(data), this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	
	
	//GetCallAssignment_UpdateDragOrder
    this.UpdateDragOrder = function (company,data) {
        var promise = $http.post(this.url + "GetCallAssignment_UpdateDragOrder",
		"sJsonInput="+JSON.stringify(data)+"&sCompany="+company,this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
    //UpdateDeliveryOrderTP
    this.UpdateDeliveryOrderTP = function (Header,Attachments,Items,AdditionalItems,company) {
        var promise = $http.post(this.url + "UpdateDeliveryOrder_TonerParts",
        "Header="+encodeURIComponent(JSON.stringify(Header))+"&Attachments="+encodeURIComponent(JSON.stringify(Attachments))+"&Items="+encodeURIComponent(JSON.stringify(Items))+"&AdditionalItems="+encodeURIComponent(JSON.stringify(AdditionalItems))+"&sCompany="+company,this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
    
	
	//UpdateDeliveryOrder
    this.UpdateDeliveryOrder = function (Header,Attachments,Items,AdditionalItems,company) {
        var promise = $http.post(this.url + "UpdateDeliveryOrder",
		"Header="+encodeURIComponent(JSON.stringify(Header))+"&Attachments="+encodeURIComponent(JSON.stringify(Attachments))+"&Items="+encodeURIComponent(JSON.stringify(Items))+"&AdditionalItems="+encodeURIComponent(JSON.stringify(AdditionalItems))+"&sCompany="+company,this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetItemList
    this.GetItemList = function (company) {
        var promise = $http.post(this.url + "GetItemList", "sCompany="+company, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetItemList
    this.GetDeliveryStatusList = function (company) {
        var promise = $http.post(this.url + "GetDeliveryStatusList", "sCompany="+company, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetDriverList
    this.GetDriverList = function (company) {
        var promise = $http.post(this.url + "GetDriverList", "sCompany="+company, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	

//GetCallAssignment
    this.GetCompletedCall = function (company,username) {
        var promise = $http.post(this.url + "GetCompletedCall", "sCompany="+company+"&sDriverName="+username, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetCallAssignment
    this.GetCompletedCallItems = function (company,username) {
        var promise = $http.post(this.url + "GetCallAssignment_CompletedItemList", "sCompany="+company+"&sDriverName="+username, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	
	//GetCallAssignment
    this.SearchCallAssignment = function (company,username,s) {
		
		var data = {
 "DriverName": username,
 "DoFromDate": s.DocDate,
 "DoToDate": s.DocDueDate,
 "DocNumber": s.DocNum,
 "SvcCall": s.SvcCall,
 "CustomerName": s.CustomerName,
 "ItemCode": s.EquipmentCode,
 "ItemName": s.EquipmentName,
 "AMPM": s.AMPM
}


        var promise = $http.post(this.url + "SearchCallAssignment", "sCompany="+company+"&sJsonInput="+JSON.stringify(data), this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetCallAssignment
    this.SearchCallAssignment_ItemList = function (company,username,s) {
		
		var data = {
 "DriverName": username,
 "DoFromDate": s.DocDate,
 "DoToDate": s.DocDueDate,
 "DocNumber": s.DocNum,
 "SvcCall": s.SvcCall,
 "CustomerName": s.CustomerName,
 "ItemCode": s.EquipmentCode,
 "ItemName": s.EquipmentName,
 "AMPM": s.AMPM
}


        var promise = $http.post(this.url + "SearchCallAssignment_ItemList", "sCompany="+company+"&sJsonInput="+JSON.stringify(data), this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };

    //getDOCusData
    this.getDOCusData = function (company,Docnumber) {
        var promise = $http.post(this.url + "GetDeliveryCustomercontactDetails", "sCompany="+company+"&sDocNum="+Docnumber, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
    
	
	//getCusData
    this.getCusData = function (company,Docnumber) {
        var promise = $http.post(this.url + "GetCustomercontactDetails", "sCompany="+company+"&sCardCode="+Docnumber, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//GetCallAssignment
    this.GetCallAssignment = function (company,username) {
        var promise = $http.post(this.url + "GetCallAssignment", "sCompany="+company+"&sDriverName="+username, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	//UpdateEAT
    this.UpdateEAT = function (EATDATA,scompany,Sjsoninput2,EATVALTIME,EATVALDATE) {
		var data = JSON.stringify({
	"Type": EATDATA.TYPE,
	"DocNum": EATDATA.DocNum,
	"DocEntry": EATDATA.DocEntry,
	"SvcCall": EATDATA.SvcCall,
	"ETADATE": EATVALDATE,
	"ETATIME": EATVALTIME,
	"Subject": EATDATA.Subject,
	"DriverName": EATDATA.DriverName,
	"Customer": EATDATA.CustomerName,
	"DriverContactNumber": EATDATA.ActivityNo,
	"CallType": "1",
	"Address": EATDATA.Address,
    "Email" : EATDATA.Email
});
        var promise = $http.post(this.url + "UpdateETA", "sCompany="+scompany+"&sJsonInput="+encodeURIComponent(data)+"&sJsonInput1="+encodeURIComponent(JSON.stringify(Sjsoninput2)), this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	
	
	
	//GetCallAssignmentItem
    this.GetCallAssignmentItem = function (company,username) {
        var promise = $http.post(this.url + "GetCallAssignment_ItemList", "sCompany="+company+"&sDriverName="+username, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	
	
	//SaveAttachments
    this.SaveAttachments = function (company,sdata) {
        var promise = $http.post(this.url + "SaveAttachments", "sJsonInput="+JSON.stringify(sdata)+"&sCompany="+company, this.config)
   .success(function (response) { if (response.returnStatus == 1) { return response;} else { return false; }});
        return promise;
    };
	
	
	
	
	
	
	//GET EAT FROM GOOGLE API 
    this.GETEATBYGOOGLE = function (d1,d2) {
		
		var Gapikey = "AIzaSyA-fUq8IZhubT6AU_KP-ZOSeEDLnKxDfAA";
		var origins = d1;
		var des = d2;
		var URL = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins="+origins+"&destinations="+des+"&key="+Gapikey
        
		var promise = $.ajax({
    type: "POST",
    url: URL,
    data: '',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function(data) {
        alert(data.d);
    },
    error: function(data){
        alert("fail");
    }
});
  
        return promise;
    };







    this.errorsomething = function (d) {
        //switch (d.error.code) {
        //    case 16: document.write("Not Saturday"); break;
        //    case 20: document.write("Not Sunday"); break;
        //    default: this.addAlert("danger", d.error.Message); break;
        //}
        if (d.error.code > 0)
        // alertify.alert(d.error.detatiledMessage);
            this.addAlert("danger", d.error.Message);

    }

    //eh = error handling
    this.eh = function (d) {
        /*console.log(d);
        if (d.returnStatus == 1 || d.error == null || d.error.code == 0) {
        return true;
        }
        else
        this.errorsomething(d);*/

        if (d.code > 0)
            this.addAlert("danger", d.message);
    }
    this.getserverURL = function () {
        return url;
    };

    this.gsid = function () {
        return $cookie.get('sessionId');
    };


        this.checkLogin = function (email, id) {
            //debugger;

            var rdata = [];
            var data = {
                "requestType": "authorisation",
                "subRequestType": "getUserInfo",
                "systemId": this.systemId,
                "authKey": "adfs3sdaczxcsdfw34",
                // "userId": this.getEmail(),
                "userId": email,
                "parameter": {
                    "clientCode": id
                }
            };
            console.log(data);
            var promise = $http.get(url + JSON.stringify(data)).success(function (response) {
                if (response.returnStatus == 1) {
                    return response;
                } else {
                    //alert('Not Connecting to server');
                    return false;
                }
            });
            return promise;
        };




    } ]);
