
loadProfessionCombobox = function (deptId) {
    $("#ProfessionID").combobox({
        url: "../Profession/ProfessionCombobox",
        queryParams: { DeptID: deptId },
        valueField: "Value",
        textField: "Text",
        multiple: false,
        onSelect: function (record) {
            loadClassCombobox(record.Value);
        }
    });
}

loadClassCombobox = function (professionId) {
    $("#ClassID").combobox({
        url: "../Class/ClassCombobox",
        queryParams: { ProfessionID: professionId },
        valueField: "Value",
        textField: "Text",
        multiple: false
    });
}
//绑定表单
bindFormEvent = function () {
    $("#file_uploadpic").uploadify({
        'debug': false,
        'preventCaching': false,
        'auto': true,
        'method': 'post',
        'buttonImage': '../Content/Images/uploadpic.png',
        'formData': {
            'folder': '../ELoad/Picture'
        },
        'swf': "../Content/uploadify/uploadify.swf",
        'queueID': "uploadfileQueuepic",
        'uploader': '../Upload/UploadList',
        'multi': false,
        'width': '80',
        'height': '20',
        'overrideEvents': ['onDialogClose'],
        'fileTypeDesc': '请选择.jpg',
        'fileTypeExts': '*.jpg',
        'fileSizeLimit': '1MB',
        'removeTimeout': 1,
        'onSelectError': function (file, errorCode, errorMsg) {
            switch (errorCode) {
                case -100:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 上传的文件数量已经超出系统限制！", 2000);
                    break;
                case -110:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 大小超出系统限制的！", 2000);
                    break;
                case -120:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 大小异常！", 2000);
                    break;
                case -130:
                    Easy.centerShow("系统消息", "文件 [" + file.name + "] 类型不正确！", 2000);
                    break;
            }
        }, 'onFallback': function () {
            Easy.centerShow("系统消息", "您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。", 2000);
        },
        'onUploadSuccess': function (file, data, response) {
            $("#photo").attr("src", data);
            $("#filePathpic").textbox("setValue", data);
            $("#uploadtab").hide();
        }
    });

    $("#DeptID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        panelWidth: 300,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#DeptID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                loadProfessionCombobox(node.id);
                loadItemTree(node.id);
            }
        }
    });
    $("#EnrollTime").datebox({
        value: Easy.GetDateTimeNow()
    });
    $("#btnChooseStudent").click(function () {
        Easy.OpenDialogEvent("#chooseStudent", "选择学生", 600, 400, "../Student/ChooseStudent", "#chooseStudent-buttons");
    });

    setTimeout(function () {
        loadItemTree(defaultDeptID);
    }, 1);

    $("#btnSaveStudent").click(function () {
        var rows = $("#choosestudentGrid").datagrid("getSelections");//选中的所有ID
        if (Easy.checkRow(rows, "学生")) {
            $("#StudentID").textbox("setValue", rows[0].StudentID);
            $("#Name").textbox("setValue", rows[0].Name + "_" + rows[0].IDCard);
            if (rows[0].Photo==="") {
                $("#uploadtab").show();
            }
            else {
                $("#photo").attr("src", rows[0].Photo);
                $("#uploadtab").hide();
            }
            $("#chooseStudent").dialog('close');

            var itemId = $("#ItemID").combotree("getValue");
            checkItemNumEvent(rows[0].StudentID, itemId);
        }
    });

    $("#ckIsForce").change(function () {
        if ($("#ckIsForce").is(":checked")) {
            $("#IsForce").textbox("setValue", "2");
        }
        else {
            $("#IsForce").textbox("setValue", "3");
        }
    });

    Easy.bindCustomPromptEvent("#ckIsForce", "mouseenter", "该学生已报考此证书，如需保存请勾选强制保存");

    bindCheckEvent();

}

//绑定显示信息的方法
bindSelectInfo = function (proveId) {
    if (proveId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Prove/SelectProve", proveId);
            var prove = JSON.parse(result.Message)[0];
            loadItemTree(prove.DeptID);
            $("#ProveID").textbox("setValue", prove.ProveID);
            $("#DeptID").combotree("setValue", prove.DeptID);
            loadProfessionCombobox(prove.DeptID);
            if (prove.ProfessionID != "0") {
                $("#ProfessionID").combobox('setValue', prove.ProfessionID);
            }
            loadClassCombobox(prove.ProfessionID);
            if (prove.ClassID != "0") {
                $("#ClassID").combobox('setValue', prove.ClassID);
            }
            $("#EnrollTime").textbox("setValue", prove.EnrollTime);
            $("#StudentID").textbox("setValue", prove.StudentID);
            $("#Name").textbox("setValue", prove.Name);
            $("#ItemID").combotree("setValue", prove.ItemID);
            $("#Remark").textbox("setValue", prove.Remark.replace(/<br \/>/g, "\r\n"));
            $("#IsForce").textbox("setValue", prove.IsForce);
            if (prove.Photo === "") {
                $("#uploadtab").show();
            }
            else {
                $("#photo").attr("src", prove.Photo);
                $("#uploadtab").hide();
            }
        }, 1);
    }
    else {
        loadProfessionCombobox(defaultDeptID);
    }
}

loadItemTree = function (deptId) {
    $("#ItemID").combotree({
        url: "../Item/ItemCombotree",
        queryParams: { DeptID: deptId, Type: "1" },
        animate: true,
        lines: true,
        editable: true,
        onSelect: function (node) {
            var tree = $(this).tree;
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                $('#ItemID').combotree('clear');
                Easy.centerShow("系统消息", "请选择末级节点", 3000);
            }
            else {
                var studentId = $("#StudentID").textbox('getValue');
                checkItemNumEvent(studentId, node.id);
            }
        },
        keyHandler: {
            query: queryHandler
        },
        onHidePanel: function (node) {
            var nodeTree = $('#ItemID').combotree('tree').tree("getSelected");
            if (nodeTree != null) {
                $("#ItemID").combotree("setValue", nodeTree.id);
            }
        }
    });
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.defaultValue = "3";
    row1.id = "#ckIsForce";
    data.jsontext.push(row1);
    Easy.checkHiddenNull(data);
}

checkItemNumEvent = function (studnetid, itemId) {
    if (studnetid != "" && itemId != "") {
        var num = 0;
        $.ajax({
            type: "post",
            url: "../Prove/SelectItemNum",
            async: false,
            data: { ProveID: ProveID, StudentID: studnetid, ItemID: itemId },
            dataType: "json",
            success: function (data) {
                num = data.Message;
            },
            error: function () {
                Easy.centerShow("系统消息", "信息加载失败", 3000);
            }
        });
        if (num > 0) {
            $("#IsForce").textbox("setValue", "3");
            $("#htmlForce").show();
            $("#ckIsForce").tooltip("show");
        }
        else {
            $("#IsForce").textbox("setValue", "1");
            $("#htmlForce").hide();
            $("#ckIsForce").tooltip("hide");
        }
    }

}

queryHandler = function (searchText, event) {
    $('#ItemID').combotree('tree').tree("search", searchText);
}

//加载表单
bindFormEvent();

bindSelectInfo(ProveID);//加载数据
