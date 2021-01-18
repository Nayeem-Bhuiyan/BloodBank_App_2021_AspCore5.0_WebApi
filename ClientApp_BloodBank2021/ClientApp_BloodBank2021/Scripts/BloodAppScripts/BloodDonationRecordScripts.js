$(document).ready(function () {
    RefreshDonationRecordList();
    ShowModal();
})



function ShowModal() {
    $("#btnShowModal").click(function() {
        $("#DonationModal").modal('show');
    })
}

function RefreshDonationRecordList() {
    $.getJSON('https://localhost:44388/api/BloodDonation/DonationRecordList', {}, function(data) {

        var tblRow = "";

        $.each(data, function(index,item) {
       tblRow+= '<tr><td>' + item.bloodReceivedBy + '</td><td>' + item.amountOfBlood + ' beg</td>'
                +'<td>' + item.donateDate + '</td><td>' + item.donatePlace + '</td><td>' + item.reasonOfDonation + '</td>'
                +'<td>' + item.donorName + '</td><td>' + item.donorContactNumber + '</td><td>' + item.address + '</td><td>' + item.age + '</td>'
                +'<td>' + item.profession + '</td><td><button type="button" class="btn btn-warning" onclick="GetEditDonationRecord(' + item.bloodDonationRecordId + ')">Edit</button><button type="button" class="btn btn-danger" onclick="DeleteDonationRecord(' + item.bloodDonationRecordId + ')">Delete</button></td></tr>'
        })

        $("#tblDonation").empty();
        $("#tblDonation").append(tblRow);
    })
}

function DeleteDonationRecord(BloodDonationRecordId) {
    var ans = confirm("Are you sure you want to Delete this!!");

    if (ans) {
        $.ajax({
            type: 'POST',
            url: 'https://localhost:44388/api/BloodDonation/DeleteDonationRecord/' + BloodDonationRecordId,
            contentType: 'application/json; charset=utf-8',
            success: function(data) {
                alert("Deleted Data Successfully!!");
                window.location.replace("/Home/DonationRecordList");
            },
            error: function (error, status, xhr) {
                console.log(error);
            console.log(status);
            console.log(xhr);
            }
        })
    }
       
}

var EditDonationRecordId = 0;

function GetEditDonationRecord(BloodDonationRecordId) {
    
    $.ajax({
        type: 'GET',
        url: 'https://localhost:44388/api/BloodDonation/GetDonationRecordById/' + BloodDonationRecordId,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            console.log(data);
                EditDonationRecordId = data.bloodDonationRecordId;
                $("#BloodReceivedBy").val(data.bloodReceivedBy);
                $("#AmountOfBlood").val(data.amountOfBlood);
                $("#DonateDate").val(data.donateDate);
                $("#DonatePlace").val(data.donatePlace);
                $("#ReasonOfDonation").val(data.reasonOfDonation);
                $("#DonorContactNumber").val(data.donorContactNumber);
                $("#DonationModal").modal('show');
          
        },
        error: function (error, status,xhr) {
            console.log(error);
            console.log(status);
            console.log(xhr);
            
        }
    })
}
function SendToDatabase() {
    var frmData = {
        BloodDonationRecordId:EditDonationRecordId ,
        BloodReceivedBy: $("#BloodReceivedBy").val() ,
        AmountOfBlood: $("#AmountOfBlood").val() ,
        DonateDate: $("#DonateDate").val() ,
        DonatePlace: $("#DonatePlace").val() ,
        ReasonOfDonation: $("#ReasonOfDonation").val() ,
        DonorContactNumber: $("#DonorContactNumber").val() ,
    }
    console.log(frmData);

        $.ajax({
            type: 'POST',
            url: 'https://localhost:44388/api/BloodDonation/InsertorUpdatedDonationRecord',
            data: JSON.stringify(frmData),
            dataType: 'json',
            contentType:'application/json; charset=utf-8',
            cache:false,
            success: function (data) {
                
                alert("Inserted Data Successfully!!");
                $("#DonationModal").modal("hide");
                window.location.reload();
                EditDonationRecordId = 0;
            },
            error: function(error) {
                console.log(error);
                EditDonationRecordId = 0;
            }
        })
    



}