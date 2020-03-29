bindFormEvent = function () {
    Easy.UpLoadFile();
    $("#Dept").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true,
        value: defaultDeptID,
        panelWidth: 300
    });

}
bindFormEvent();