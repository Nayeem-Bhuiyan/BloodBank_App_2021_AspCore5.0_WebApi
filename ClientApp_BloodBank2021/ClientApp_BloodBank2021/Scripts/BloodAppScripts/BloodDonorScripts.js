$(document).ready(function () {
    RefreshDonorRegistrationList();
    
});

function GotoCreatePage (){
    window.location.href = "/Home/CreateDonorRegistration";
}

function RefreshDonorRegistrationList() {
    $.getJSON('https://localhost:44388/api/BloodDonor/GetDonorList', {}, function (data) {
        var tblRow = "";

        $.each(data, function (index, item) {
            tblRow += '<tr><td>' + item.donorName + '</td><td>' + item.fatherName + '</td><td>' + item.bloodGroupName + '</td><td>' + item.donorContactNumber + '</td><td>' + item.address + '</td><td>' + item.age + '</td><td>' + item.height + ' feet</td>'
                + '<td>' + item.weight + ' Kg</td><td>' + item.niDorBirthId + '</td><td>' + item.birthDate + '</td><td>' + item.email + '</td>'
                + '<td>' + item.profession + '</td><td>' + item.lastWorkPlace + '</td><td>' + item.majorPhysicalProblem + '</td><td>' + item.smocker + '</td>'
                + '<td>' + item.drugAddicted + '</td><td>' + item.bloodDonateExperience + '</td><td>' + item.majorOperation+'</td><td>' + item.countryName + '</td>'
                + '<td>' + item.districtName + '</td><td>' + item.thanaName + '</td><td>' + item.genderName + '</td><td>' + item.religionName + '</td>'
                + '<td><button type="button" class="btn btn-warning" onclick="GetEditDonorRecord(' + item.donorContactNumber + ')">Edit</button>|<button type="button" class="btn btn-danger" onclick="DeleteDonor(' + item.donorContactNumber + ')">Delete</button></td></tr>'
        })

        $("#tblDonorList").empty();
        $("#tblDonorList").append(tblRow);
    })
}


function GetEditDonorRecord(donorContactNumber) {

}

function DeleteDonor(donorContactNumber) {

}

function CreateNewDonor() {

    var mPhysicalProblem = $("input[name='MajorPhysicalProblem']:checked").val();
    var smocker = $("input[name='Smocker']:checked").val();
    var drugAddicted = $("input[name='DrugAddicted']:checked").val();
    var bloodDonateExperience = $("input[name='BloodDonateExperience']:checked").val();
    var majorOperation = $("input[name='MajorOperation']:checked").val();
  

    var frmData = {
        DonorContactNumber: $("#DonorContactNumber").val(),
        DonorName: $("#DonorName").val(),
        FatherName: $("#FatherName").val(),
        Address: $("#Address").val(),
        Height: $("#Height").val(),
        Weight: $("#Weight").val(),
        NIDorBirthId: $("#NIDorBirthId").val(),
        BirthDate: $("#BirthDate").val(),
        Email: $("#Email").val(),
        Profession: $("#Profession").val(),
        LastWorkPlace: $("#LastWorkPlace").val(),

        MajorPhysicalProblem:mPhysicalProblem,
        Smocker: smocker,
        DrugAddicted: drugAddicted,
        BloodDonateExperience: bloodDonateExperience,
        MajorOperation: majorOperation,

        ThanaId: $("#dptThana").val(),
        GenderId: $("#dptGender").val(),
        ReligionId: $("#dptReligion").val(),
        BloodGroupId: $("#dptBloodGroup").val(),

       
    }
    console.log(frmData);

      var donorData  =JSON.stringify(frmData)

    $.ajax({
        url:"https://localhost:44388/api/BloodDonor/InsertOrUpdateDonor",
        type:"POST",
        data:donorData,
        async: true,
        dataType:"json",
        cache: false,
        processData: false,
        contentType:"application/json; charset=utf-8",
        success:function (data) {
            alert(data.successMessage);
            //window.location.href = "/Home/DonorRegistrationList";
        },
        error: function(error) {
            console.log(error);
            if (error.errorMessage==="undefined") {
                alert("Client Error 400!!!");
            }
            //window.location.href = "/Home/DonorRegistrationList";
        }
    })


}