var StudentID = "0";//定义ID
firstFunction = function () {
    //加载表格
    initTable({ MenuID: menuId });

    //加载搜索表单数据
    bindSearchFormEvent();

    //加载搜索按钮的点击事件
    bindSearchClickEvent();

    //加载开关高级搜索事件
    Easy.bindOpenCloseSearchBoxEvent(90);
    bindSaveUplodMethod();
    //点击添加保存按钮
    Easy.bindSaveButtonClickEvent("#editStudent", "#fSave", "../Student/GetStudentEdit", "#btnSave", "1", "#grid", menuId, "1", "#StudentID");
    Easy.bindSaveButtonClickEvent("#editNoStudent", "#fNoSave", "../Student/GetStudentEditNo", "#btnNoSave", "1", "#grid", menuId, "1", "#noStudentID");
    Easy.bindSaveButtonClickEvent("#editStudentIDCard", "#fIDCardSave", "../Student/GetStudentEditIDCard", "#btnSaveIDCard", "1", "#grid", menuId, "1", "#idStudentID");
    Easy.bindSaveButtonClickEvent("#modifyStudent", "#fModifySave", "../StudentInfo/GetStudentInfoEdit", "#btnSaveModify", "1", "#grid", menuId, "1", "#studentInfoId");
    bindSaveUploadPicture();
}

//加载表格
initTable = function (queryData) {
    $("#grid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "StudentID", hidden: true },
            { field: "DeptName", title: "校区", width: 120, sortable: true },
            { field: "Name", title: "姓名", width: 80, sortable: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 150, sortable: true },
            { field: "Sex", title: "性别", width: 50, sortable: true },
            { field: "Mobile", title: "联系电话", width: 100, sortable: true },
            { field: "QQ", title: "QQ号", width: 100, sortable: true, },
            { field: "WeChat", title: "微信号", width: 100, sortable: true, },
            { field: "Address", title: "地址", width: 150, sortable: true, },
            { field: "Status", title: "状态", width: 60, sortable: true }
        ]],
        url: "../Student/GetStudentList", sortName: "StudentID", sortOrder: "desc",
        toolbar: Easy.loadToolbar(menuId, "1"),
        rowStyler: function (index, row) {
            if (row.StatusValue === "2") {
                return "color:#ff0000;";
            }
        }
    });
}

//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btnSearch").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initTable(queryData);//将值传递给
    });
    $("#btnReset").click(function () {
        $("#fSearch").form("reset");//重置表单
        initTable({ MenuID: menuId });
    });
}

//绑定搜索表单数据
bindSearchFormEvent = function () {
    $("#selDept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "No", Status: "1" },
        animate: true,
        lines: true,
        panelWidth: 300
    });
}

//添加
add = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "add")) {
        StudentID = "0";
        Easy.OpenDialogEvent("#editStudent", "编辑交费人", 600, 380, "../Student/StudentEdit", "#editStudent-buttons");
    }
}
modify = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "交费人")) {
            StudentID = rows[0].StudentID;
            Easy.OpenDialogEvent("#modifyStudent", "修改相关信息", 700, 380, "../Student/StudentModify", "#modifyStudent-buttons");
        }
    }
}
//修改
edit = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "edit")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "交费人")) {
            StudentID = rows[0].StudentID;
            Easy.OpenDialogEvent("#editStudent", "编辑交费人", 600, 380, "../Student/StudentEdit", "#editStudent-buttons");
        }
    }
}
view = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "view")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "交费人")) {
            StudentID = rows[0].StudentID;
            Easy.OpenDialogEvent("#viewStudent", "查看交费人", 800, 600, "../Student/StudentView", "#viewStudent-buttons");
        }
    }
}
//启用
enable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "enable")) {
        bindUpdateStatus("启用", "1");
    }
}

//停用
disable = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "disable")) {
        bindUpdateStatus("停用", "2");
    }
}

bindUpdateStatus = function (confirmstr, status) {
    var rows = $("#grid").datagrid("getSelections");//选中的所有ID
    //验证是否选中行
    if (Easy.checkRow(rows, "交费人信息")) {
        //修改状态
        Easy.bindUpdateValue(confirmstr, status, "../Student/GetUpdateStudentStatus", rows[0].StudentID, "1", "#grid");
    }
}
bindSaveUplodMethod = function () {
    $("#btnSaveup").click(function () {
        var data = $("#gridStudent").datagrid("getData");
        if (data.rows.length === 0) {
            Easy.centerShow("系统消息", "没有交费人信息", 2000);
            return false;
        }
        var flag = Easy.bindSaveUploadFileForm("#fUpload", "../Student/UploadStdent", "#btnSaveup", "#grid", "1", "upload", "#gridStudent", data.rows, "#stuLayout");
    });
}
upload = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
        Easy.OpenDialogEvent("#upStudent", "导入交费人", 800, 600, "../Student/StudentUpload", "#upStudent-buttons");
    }
}

addno = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "addno")) {
        StudentID = "0";
        Easy.OpenDialogEvent("#editNoStudent", "编辑无身份证交费人", 600, 380, "../Student/StudentEditNo", "#editNoStudent-buttons");
    }
}

editno = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "editno")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "交费人")) {
            StudentID = rows[0].StudentID;
            Easy.OpenDialogEvent("#editNoStudent", "编辑无身份证交费人", 600, 380, "../Student/StudentEditNo", "#editNoStudent-buttons");
        }
    }
}

editidcard = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "editidcard")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "交费人")) {
            if (rows[0].IDCard === "") {
                StudentID = rows[0].StudentID;
                Easy.OpenDialogEvent("#editStudentIDCard", "补录身份证", 400, 180, "../Student/StudentEditIDCard", "#editStudentIDCard-buttons");
            }
            else {
                Easy.centerShow("系统消息", "该交费人已录入身份证", 3000);
            }
        }
    }
}

deriveTemplate = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "deriveTemplate")) {
        location.href = "../ModelExcel/交费人信息导入模板.xls";
    }
};
derive = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "derive")) {
        var queryData = rerurnQueryData();
        $.ajax({
            type: "post",
            url: "../Student/DownloadStudent",
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

rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        txtName: $("#txtName").textbox("getValue"),
        txtIDCard: $("#txtIDCard").textbox("getValue"),
        txtMobile: $("#txtMobile").textbox("getValue"),
        selDept: $("#selDept").combotree("getValue")
    }
    return queryData;
}
uploadpic = function () {
    if (Easy.bindPowerValidationEvent(menuId, "1", "uploadpic")) {
        var rows = $("#grid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "交费人")) {
            StudentID = rows[0].StudentID;
            Easy.OpenDialogEvent("#upStudentPic", "上传: <span style='color:red'>" + rows[0].Name + "-" + rows[0].IDCard + "</span> 照片", 800, 600, "../Student/StudentPicture", "#upStudentPic-buttons");
        }
    }
};

bindSaveUploadPicture = function () {
    $("#btnSaveupPic").click(function () {
        if ($("#btnSaveupPic").linkbutton("options").disabled === false) {//按钮没有禁用才能执行（easyui特殊处理）
            if (Easy.bindPowerValidationEvent(menuId, "1", "upload")) {
                $("#fUploadpic").form("submit", {
                    url: "../Student/UploadStudentPicture",
                    onSubmit: function () {
                        var validate = $("#fUploadpic").form("validate");//验证
                        if (validate) {
                            $("#btnSaveupPic").linkbutton("disable");//禁用按钮
                        }
                        return validate;
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.IsError === false) {
                            $("#upStudentPic").dialog('close');
                            Easy.centerShow("系统消息", result.Message, 2000);
                        }
                        else {
                            Easy.centerShow("系统消息", result.Message, 2000);
                        }
                        $("#btnSaveupPic").linkbutton("enable");//解除按钮禁用
                    }
                });
            }
        }
    });
};