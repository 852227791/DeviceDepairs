firstFunction = function () {
    bindSearchFormEvent();
    bindSearchClickEvent();
    bindGridTableMethod({ MenuID: menuId });
    Easy.bindOpenCloseSearchBoxEvent(90);
};
bindSearchFormEvent = function () {
    $("#selDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
}

bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = getSearchData();
        bindGridTableMethod(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        bindGridTableMethod({ MenuID: menuId });
    });
}
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = getSearchData();
        $.ajax({
            type: "post",
            url: "../ChangeEnroll/GetChangeProfessionDownload",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}
bindGridTableMethod = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: false,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "sEnrollsProfessionsOrderID", checkbox: true },
            { field: "DeptName", title: "校区", width: 150, sortable: true },
            { field: "StuName", title: "姓名", width: 100, sortable: true },
            { field: "EnrollNum", title: "学号", width: 100, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "ProName", title: "专业", width: 135, sortable: true },
            { field: "NumName", title: "缴费次数", width: 60, sortable: true },
            { field: "ShouldMoney", title: "应供贷金额",  sortable: true, align: 'right' },
            { field: "PaidMoney", title: "实际供贷金额", sortable: true, align: 'right' },
            { field: "IsNumItem", title: "是否已读", width: 60, sortable: true }
        ]],
        url: "../ChangeEnroll/GetChangeProfessionExceptionData", sortName: "sEnrollsProfessionsOrderID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1")
    });
}

getSearchData = function () {
    var queueData = {
        MenuID: menuId,
        name: $("#txtName").textbox("getValue"),
        idCard: $("#txtIDCard").textbox("getValue"),
        enrollNum: $("#txtEnrollNum").textbox("getValue"),
        deptId:$("#selDept").combotree("getValue")
    };
    return queueData;
};
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRows(rows, "转专业异常信息")) {
            $.messager.confirm('确认', '您确认要转为已读吗？', function (r) {
                if (r) {
                    var id = "";
                    for (var i = 0; i < rows.length; i++) {
                        id += rows[i].sEnrollsProfessionsOrderID + ",";
                    }
                    id = id.substring(0, id.length - 1);
                    var result = Easy.bindSelectInfo("../ChangeEnroll/ChangeIsNumItem", id);
                    Easy.centerShow("系统消息", result.Message, 2000);
                    $('#grid').datagrid('reload');
                }
            });
          

        }
    }
}
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = getSearchData();
        $.ajax({
            type: "post",
            url: "../ChangeEnroll/GetChangeProfessionExceptionDownload",
            async: false,
            data: queryData,
            dataType: "json",
            success: function (result) {
                if (result.IsError === false) {
                    location.href = "../Temp/" + result.Message;
                }
            }
        });
    }
}