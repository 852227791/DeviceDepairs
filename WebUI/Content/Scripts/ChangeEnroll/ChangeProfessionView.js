
Easy.bindSelectStudentInfo(selectRow.StudentID);
bindChangeProfessionMethod = function () {
    $("#chName").html(selectRow.StudName);
    $("#chEnrollNum").html(selectRow.EnrollNum);
    $("#chIDCard").html(selectRow.IDCard);
    $("#chBeSchool").html(selectRow.BeSchool);
    $("#chBeYear").html(selectRow.BeYear);
    $("#chBeMonth").html(selectRow.BeMonth);
    $("#chBeLevel").html(selectRow.BeLevel);
    $("#chBeMajor").html(selectRow.BeMajor);
    $("#chAfSchool").html(selectRow.AfSchool);
    $("#chAfYear").html(selectRow.AfYear);
    $("#chAfMonth").html(selectRow.AfMonth);
    $("#chAfLevel").html(selectRow.AfLevel);
    $("#chAfMajor").html(selectRow.AfMajor);
    $("#chChangeTime").html(selectRow.ChangeTime);
    $("#chChangeReson").html(selectRow.ChangeReson);
    $("#chSortName").html(selectRow.SortName);
}
bindChangeProfessionMethod();
bindsFeeTotalMethod = function () {
    debugger;
    var result1 = Easy.bindSelectInfo("../ChangeEnroll/GetFeeinfo", selectRow.sEnrollsProfessionID);
    var data1 = JSON.parse(result1.Data);
    var row1 = data1[0];
    $("#chBeShouldMoeny").html(row1.ShouldMoney + "元");
    $("#chBePadiMoney").html(row1.PaidMoney + "元");
    $("#chBeDiscountMoney").html(row1.DiscountMoney + "元");
    $("#chBeOffsetMoney").html(row1.OffsetMoney + "元");
    $("#chBeByOffsetMoney").html(row1.ByOffsetMoney + "元");

    var result2 = Easy.bindSelectInfo("../ChangeEnroll/GetFeeinfo", selectRow.NewsEnrollsProfessionID);
    var data2 = JSON.parse(result2.Data);
    var row2 = data2[0];
    $("#chAfShouldMoeny").html(row2.ShouldMoney + "元");
    $("#chAfPadiMoney").html(row2.PaidMoney + "元");
    $("#chAfDiscountMoney").html(row2.DiscountMoney + "元");
    $("#chAfOffsetMoney").html(row2.OffsetMoney + "元");
    $("#chAfByOffsetMoney").html(row2.ByOffsetMoney + "元");
}

bindsFeeTotalMethod();