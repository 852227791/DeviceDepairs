bindSelectsEnrollInfo = function () {
    var result = Easy.bindSelectInfo("../sEnroll/SelectsEnroll", sEnrollsProfessionID);
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
    $("#spStatus").html($("#grid").datagrid("getSelected").StatusName);
    $("#spRemark").html(enroll.Remark);
    var result1 = Easy.bindSelectInfo("../sEnroll/GetsOrderStatus", sEnrollsProfessionID);
    $("#spsOrderStatus").html(result1.Message);
}

bindSelectsFeeInfo = function () {
    $("#SchemeTab .tabs-header-left .tabs li").each(function (index, obj) {
        var tab = $(".tabs-title", this).text();
        $("#SchemeTab").tabs('close', tab);
    });
    var result = Easy.bindSelectInfo("../sEnroll/GetSchemeJson", sEnrollsProfessionID);
    var jsonData = JSON.parse(result.Message);
    if (jsonData.length > 0) {
        for (var i = 0; i < jsonData.length; i++) {
            var semesterData = "[]";
            var sorderData = "[]";
            var sorderGiveData = "[]";
            //添加缴费方案tab
            $('#SchemeTab').tabs('add', {
                title: jsonData[i].text,
                content: '<div id="semester_' + jsonData[i].id + '" class="easyui-tabs" data-options="border:false"></div>'
            });
            //得到缴费方案缴费期数
            $.ajax({
                type: "post",
                url: "../sEnroll/GetSemesterJson",
                async: false,
                data: { MajorID: sEnrollsProfessionID, PlanItemID: jsonData[i].id },
                dataType: "json",
                success: function (data) {
                    semesterData = data.Message;
                },
                error: function () {
                    Easy.centerShow("系统消息", "信息加载失败", 3000);
                }
            });
            //得到缴费方案缴费单
            $.ajax({
                type: "post",
                url: "../sEnroll/GetSchemeOrderJson",
                async: false,
                data: { MajorID: sEnrollsProfessionID, PlanItemID: jsonData[i].id },
                dataType: "json",
                success: function (data) {
                    sorderData = data.Message;
                },
                error: function () {
                    Easy.centerShow("系统消息", "信息加载失败", 3000);
                }
            });
            //得到缴费方案配品
            $.ajax({
                type: "post",
                url: "../sEnroll/GetSchemeOrderGiveJson",
                async: false,
                data: { MajorID: sEnrollsProfessionID, PlanItemID: jsonData[i].id },
                dataType: "json",
                success: function (data) {
                    sorderGiveData = data.Message;
                },
                error: function () {
                    Easy.centerShow("系统消息", "信息加载失败", 3000);
                }
            });
            //处理缴费方案期数数据
            var jsonSemesterData = JSON.parse(semesterData);
            if (jsonSemesterData.length > 0) {
                //添加期数tab
                for (var a = 0; a < jsonSemesterData.length; a++) {
                    $('#semester_' + jsonData[i].id + '').tabs('add', {
                        title: jsonSemesterData[a].NumName,
                        content: '<table id="semester_table_' + jsonSemesterData[a].NumItemID + '"></table>'
                    });

                    $('#semester_table_' + jsonSemesterData[a].NumItemID + '').datagrid({
                        striped: true,//斑马线
                        rownumbers: true,//行号
                        singleSelect: true,//只允许选择一行
                        pagination: false,//分页
                        showFooter: true,
                        pageSize: 20,
                        columns: [[
                            { field: "Name", title: "费用项", width: 150, sortable: true },
                            { field: "ShouldMoney", title: "应供贷金额", width: 110, sortable: true },
                            { field: "PaidMoney", title: "实际供贷金额", width: 110, sortable: true },
                            { field: "NoPayMoney", title: "未供贷金额", width: 110, sortable: true },
                            { field: "LimitTime", title: "缴费期限", width: 80, sortable: true }
                        ]]
                    });

                    //绑定期数费用项
                    var jsonOrderData = JSON.parse(sorderData);
                    if (jsonOrderData.length > 0) {
                        var shouldMoney = 0;
                        var paidMoney = 0;
                        var upPaidMoney = 0;
                        for (var n = 0; n < jsonOrderData.length; n++) {
                            if (jsonOrderData[n].NumItemID == jsonSemesterData[a].NumItemID) {
                                shouldMoney = parseFloat(shouldMoney) + parseFloat(jsonOrderData[n].ShouldMoney);
                                paidMoney = parseFloat(paidMoney) + parseFloat(jsonOrderData[n].PaidMoney);
                                upPaidMoney = parseFloat(upPaidMoney) + parseFloat(jsonOrderData[n].NoPayMoney);


                                $('#semester_table_' + jsonSemesterData[a].NumItemID + '').datagrid('appendRow', {
                                    Name: jsonOrderData[n].Name,
                                    ShouldMoney: jsonOrderData[n].ShouldMoney,
                                    PaidMoney: jsonOrderData[n].PaidMoney,
                                    NoPayMoney: jsonOrderData[n].NoPayMoney,
                                    LimitTime: jsonOrderData[n].LimitTime
                                });
                            }
                        }
                        $('#semester_table_' + jsonSemesterData[a].NumItemID + '').datagrid('reloadFooter',[
                            { ShouldMoney: '合计：' + shouldMoney.toFixed("2"), PaidMoney: '合计：' + paidMoney.toFixed("2"), NoPayMoney: '合计：' + upPaidMoney.toFixed("2") }
                       ] );
                    }
                }
                //添加配品tab
                var jsonOrderGiveData = JSON.parse(sorderGiveData);
                if (jsonOrderGiveData.length > 0) {
                    $('#semester_' + jsonData[i].id + '').tabs('add', {
                        title: '配品',
                        content: '<table id="give_table_' + jsonData[i].id + '"></table>'
                    });

                    $('#give_table_' + jsonData[i].id + '').datagrid({
                        striped: true,//斑马线
                        rownumbers: true,//行号
                        singleSelect: true,//只允许选择一行
                        pagination: false,//分页
                        pageSize: 20,
                        columns: [[
                            { field: "Name", title: "配品名称", width: 300, sortable: true }
                        ]]
                    });

                    //绑定配品
                    for (var t = 0; t < jsonOrderGiveData.length; t++) {
                        $('#give_table_' + jsonData[i].id + '').datagrid('appendRow', {
                            Name: jsonOrderGiveData[t].Name
                        });
                    }
                }
                $('#semester_' + jsonData[i].id + '').tabs('select', 0);
            }
        }
        $('#SchemeTab').tabs('select', 0);
    }
}

tabsLoad = function (i) {
    $('#ViewInfo').tabs({
        border: false,
        selected: i,
        onSelect: function (title, index) {
            if (index === 0) {
                bindSelectsEnrollInfo();
            }
            else if (index === 1) {
                Easy.bindSelectStudentInfo($("#grid").datagrid("getSelected").StudentID);
            }
            else if (index === 2) {
                bindSelectsFeeInfo();
            }
        }
    });
}

tabsLoad(0);