$(document).ready(function () {
    RefreshBloodUserList();
    ShowModal();
    LoadBloodGroup(); 

    $("#btnSave").click(function() {
        SaveToDatabase();
    })
});

function ShowModal() {
    $("#btnShowModal").click(function () {
        $("#UserModal").modal('show');
    })
}

function LoadBloodGroup() {
    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadBloodGroupList', {}, function (data) {
        var option = '';

        $.each(data, function (index, item) {
            option += '<option value="' + item.bloodGroupId + '">' + item.bloodGroupName + '</option>';
        })

        $("#dptBloodGroup").empty();
        $("#dptBloodGroup").prepend('<option value="">Select Blood Group</option>');
        $("#dptBloodGroup").append(option);



    })
}

function RefreshBloodUserList() {
    $.getJSON('https://localhost:44388/api/BloodUsers/GetBloodRequestUserList', {}, function(data) {
        var tblRow = "";
        $.each(data, function(index,item) {
            tblRow += '<tr><td>' + item.userName + '</td><td>' + item.userMobile + '</td><td>' + item.userAddress + '</td><td>' + item.reasonOfBlood + '</td><td>' + item.amountOfBloodRequested +'Beg</td><td>' + item.date_BloodRequest + '</td><td>' + item.date_BloodNeed + '</td><td>' + item.bloodGroupName + '</td><td><button type="button" class="btn btn-warning" onclick="GetEditBloodUser(' + item.bloodRequestUserId + ')">Edit</button>|<button type="button" class="btn btn-danger" onclick="DeleteBloodUser(' + item.bloodRequestUserId +')">Delete</button></td></tr>';
        })


        $("#tblBloodUserData").empty();
        $("#tblBloodUserData").append(tblRow);
    })
}


function DeleteBloodUser(bloodRequestUserId) {
    var ans = confirm("Are you sure you want to delete this!!");

    if (ans) {
        $.post("https://localhost:44388/api/BloodUsers/DeleteBloodUserRecordById/" + bloodRequestUserId, {}, function (data) {
            window.location.reload();

        }, function (error) {
            console.log(error);
        })
    }
}

var EditBloodRequestUserId = 0;

function GetEditBloodUser(bloodRequestUserId) {
    $.getJSON("https://localhost:44388/api/BloodUsers/GetBloodRequestUserById/" + bloodRequestUserId, {}, function(data) {

        EditBloodRequestUserId = data.bloodRequestUserId;
        $("#UserName").val(data.userName);
        $("#UserMobile").val(data.userMobile);
        $("#UserAddress").val(data.userAddress);
        $("#ReasonOfBlood").val(data.reasonOfBlood);
        $("#AmountOfBloodRequested").val(data.amountOfBloodRequested);
        $("#Date_BloodRequest").val(data.date_BloodRequest);
        $("#Date_BloodNeed").val(data.date_BloodNeed);
        $("#dptBloodGroup").val(data.bloodGroupId);
    }, function(error) {
            console.log(error);
    })
}


function SaveToDatabase() {
    var frmData = {
                    BloodRequestUserId: EditBloodRequestUserId ,
                    UserName: $("#UserName").val(),
                    UserMobile: $("#UserMobile").val(),
                    UserAddress: $("#UserAddress").val(),
                    ReasonOfBlood: $("#ReasonOfBlood").val(),
                    AmountOfBloodRequested: $("#AmountOfBloodRequested").val(),
                    Date_BloodRequest: $("#Date_BloodRequest").val(),
                    Date_BloodNeed: $("#Date_BloodNeed").val(),
                    BloodGroupId: $("#dptBloodGroup").val()
                 }


    $.ajax({
        type: "POST",
        url: "https://localhost:44388/api/BloodUsers/InsertOrUpdateBloodUser",
        data: JSON.stringify(frmData),
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        cache: false,
        success: function(data) {
            alert(data);
            window.location.reload();
            EditBloodRequestUserId = 0;
        },
        error: function(error) {
            console.log(error);
            EditBloodRequestUserId = 0;

        }
    })


   }




