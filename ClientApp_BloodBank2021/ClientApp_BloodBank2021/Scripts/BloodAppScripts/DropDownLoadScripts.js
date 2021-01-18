$(document).ready(function () {
    LoadBloodGroup();
    LoadCountry();
    LoadGender();
    LoadReligion();
});

function LoadCountry() {
    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadCountryList', {}, function(data) {
        var option = '';

        $.each(data, function(index,item) {
            option+= '<option value="' + item.countryId + '">' + item.countryName +'</option>';
        })

        $("#dptCountry").empty();
        $("#dptCountry").prepend('<option value="">Select Country</option>');
        $("#dptCountry").append(option);
       


    })
}

$("#dptCountry").mouseup(function() {
    var id = $("#dptCountry").val();

    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadDistrictListByCountryId/' + id, {}, function (data) {
        var option = '';

        $.each(data, function (index, item) {
            option += '<option value="' + item.districtId + '">' + item.districtName + '</option>';
        })

        $("#dptDistrict").empty();
        $("#dptDistrict").prepend('<option value="">Select District</option>');
        $("#dptDistrict").append(option);
        


    })

})


$("#dptDistrict").mouseup(function () {
    var id = $("#dptDistrict").val();

    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadThanaListByDistrictId/' + id, {}, function (data) {
        var option = '';

        $.each(data, function (index, item) {
            option += '<option value="' + item.thanaId + '">' + item.thanaName  + '</option>';
        })

        $("#dptThana").empty();
        $("#dptThana").prepend('<option value="">Select Thana</option>');
        $("#dptThana").append(option);
        


    })

})







function LoadReligion() {
    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadReligionList', {}, function (data) {
        var option = '';

        $.each(data, function (index, item) {
            option += '<option value="' + item.religionId + '">' + item.religionName  + '</option>';
        })

        $("#dptReligion").empty();
        $("#dptReligion").prepend('<option value="">Select Religion</option>');
        $("#dptReligion").append(option);
        


    })
}


function LoadGender() {
    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadGenderList', {}, function (data) {
        var option = '';

        $.each(data, function (index, item) {
            option += '<option value="' + item.genderId + '">' + item.genderName  + '</option>';
        })

        $("#dptGender").empty();
        $("#dptGender").prepend('<option value="">Select Gender</option>');
        $("#dptGender").append(option);
        


    })
}


function LoadBloodGroup() {
    $.getJSON('https://localhost:44388/api/CommonDataLoad/LoadBloodGroupList', {}, function (data) {
        var option = '';

        $.each(data, function (index, item) {
            option += '<option value="' + item.bloodGroupId + '">' + item.bloodGroupName  + '</option>';
        })

        $("#dptBloodGroup").empty();
        $("#dptBloodGroup").prepend('<option value="">Select Blood Group</option>');
        $("#dptBloodGroup").append(option);
        


    })
}