var selectRow;

initFunction = function () {
    selectRow = $("#grid").datagrid("getSelected");
}

bindsOrderInfo = function () {
    $("#spPlanName").html(selectRow.PlanName);
    $("#spNumName").html(selectRow.NumName);
    $("#spFeeType").html(selectRow.FeeType);
    $("#spFeeName").html(selectRow.FeeName);
    $("#spLimitTime").html(selectRow.LimitTime);
    $("#spShouldMoney").html(selectRow.ShouldMoney);
    $("#spPaidMoney").html(selectRow.PaidMoney);
    $("#spStatusName").html(selectRow.StatusName);
    $("#spCreateTime").html(selectRow.CreateTime);
}

bindSelectsEnrollInfo = function () {
    var result = Easy.bindSelectInfo("../sEnroll/SelectsEnroll", selectRow.sEnrollsProfessionID);
    var enroll = JSON.parse(result.Message)[0];
    $("#spDept").html(enroll.DeptName);
    $("#spEnrollNum").html(enroll.EnrollNum);
    $("#spStudent").html(enroll.StuName);
    $("#spYear").html(enroll.YearName);
    $("#spMonth").html(enroll.MonthName);
    $("#spLevel").html(enroll.LevelName);
    $("#spMajor").html(enroll.MajorName);
    $("#spEnrollTime").html(enroll.EnrollTime);
    $("#spsFeeScheme").html(enroll.SchemeName);
    $("#spRemark").html(enroll.Remark);
}

tabsLoad = function (i) {
    $('#ViewInfo').tabs({
        border: false,
        selected: i,
        onSelect: function (title, index) {
            if (index == 0) {
                bindsOrderInfo();
            }
            else if (index === 1) {
                bindSelectsEnrollInfo();
            }
            else if (index === 2) {
                Easy.bindSelectStudentInfo(selectRow.StudentID);
            }
        }
    });
}

initFunction();

tabsLoad(0);