class EmployeePage {
    // Hàm khởi tạo
    constructor(gridId) {
        let me = this;

        // Lưu lại grid đang thao tác
        me.grid = $(`#${gridId}`);
        
        // Đặt giá trị đầu tiên cho pageNumber
        me.pageNumber = 1;

        // khởi tạo form detail
        me.initFormDetail();

        // Khởi tạo form confirm
        me.initConfirmForm();

        // Hàm để khởi tạo sự kiện
        me.initEvents();

        // lấy ra cấu hình các cột
        me.columnConfig = me.getColumnConfig();

        // Lấy dữ liệu nhân viên
        me.getEmployeeData();

        // Lấy dữ liệu vị trí
        me.getPositionData();

        // Lấy dữ liệu phòng ban
        me.getDepartmentData();
    }

    /**
     * Hàm dùng để lấy dữ liệu cho select box position
     * BTHOANG
     */
    getPositionData(){
        let me = this,
            url = Resource.APIs.Positions;

        CommonFn.Ajax(url, Resource.Method.Get, {}, function (res) {
            if (res) {
                me.positionSelectGendering(res)
            } else {
                console.log("Có lỗi khi lấy dữ liệu từ server");
            }
        }, function(error){
            CommonFn.toast('toast-messenger', Resource.Toast.success, `Lỗi: '${error}' khi lấy dữ liệu vị trí. Vui lòng liên hệ để được sửa chữa.`);
        });
    }

    /**
     * Hàm dùng lấy dữ liệu cho select box department
     * BTHOANG
     */
    getDepartmentData(){
        let me = this,
            url = Resource.APIs.Departments;

        CommonFn.Ajax(url, Resource.Method.Get, {}, function (res) {
            if (res) {
                me.departmentSelectGendering(res);
            } else {
                console.log("Có lỗi khi lấy dữ liệu từ server");
            }
        }, function(error){
            CommonFn.toast('toast-messenger', Resource.Toast.error, `Lỗi: '${error}' khi lấy dữ liệu phòng ban. Vui lòng liên hệ để được sửa chữa.`);
        });
    }

    /**
    * Hàm gán positon vào checkbox
    * BTHOANG
    */
    positionSelectGendering(data){
        let me = this,
            positionSelect = $('[SelectID = "position-select"]');

        positionSelect.html("");
        positionSelect.append('<option value="" selected>Tất cả vị trí</option>');
        data.filter(function(item){
            positionSelect.append(`<option value="${item.positionID}">${item.positionName}</option>`);
        });
        CommonFn.toast('toast-messenger', Resource.Toast.success, 'Đã tải xong dữ liệu của các vị trí.');
    }

    /**
     * Hàm gán department vào checkbox
     * BTHOANG
     */
    departmentSelectGendering(data){
        let me = this,
            departmentSelect = $('[SelectID = "department-select"]');

        departmentSelect.html("");
        departmentSelect.append('<option value="" selected>Tất cả phòng ban</option>');
        data.filter(function(item){
            departmentSelect.append(`<option value="${item.departmentID}">${item.departmentName}</option>`);
        });
        CommonFn.toast('toast-messenger', Resource.Toast.success, 'Đã tải xong dữ liệu của các phòng ban.');
    }

    /**
     * Hàm tải lại dữ liệu
     * BTHOANG
     */
    reloadData(){
        let me = this;

        me.getEmployeeData();

        $(".delete-btn").prop('disabled', true);
        $(".replication-btn").prop('disabled', true);
    }

    /**
     * Hàm khởi tạo sự kiện
     * BTHOANG
     */
    initEvents() {
        let me = this;

        // Khởi tạo sự kiện cho table
       me.initEventsTable();

       // khởi tạo sự kiện cho toolbar
        me.initEventsToolbar();

        // Khởi tạo sự kiện cho navigation
        me.initEventsNavigation();

        // Khởi tạo sự kiện cho bộ lọc và tìm kiếm
        me.initEventsFilter();
    }

    /**
     * Hàm lấy mã nhân viên mới tự động tăng
     * BTHOANG
     */
    getNewEmployeeCode(param){
        let me = this,
            url = `${Resource.APIs.Employees}/new-code`;

        CommonFn.Ajax(url, Resource.Method.Get, {}, function (response) {
            if (response) {
                me.renderNewCode(response, param);
            } else {
                console.log("Có lỗi khi lấy dữ liệu từ server");
            }
        }, function(error){
            CommonFn.toast('toast-messenger', Resource.Toast.error, `Lỗi: '${error}' khi lấy mã nhân viên mới. Vui lòng liên hệ để được sửa chữa.`);
        });
    }
    
    renderNewCode(data, param){
        let me = this,
            newCode = {};
        Object.assign(newCode, data);

        param.newEmployeeCode = newCode.employeeCode;

        // kiểm tra có tồn tại form detail
        if(me.formDetail){
            me.formDetail.open(param);
        }
    }

    /**
     * Hàm khởi tạo sự kiện cho bộ lọc và tìm kiếm
     * BTHOANG
     */
    initEventsFilter(){
        let me = this;

        // Hàm khởi tạo sự kiện tìm kiếm
        me.searchFilter();

        // Hàm khởi tạo sự kiện bộ lọc vị trí
        me.positionFilter();

        // Hàm khởi tạo sự kiện bộ lọc phòng ban
        me.departmentFilter();
    }

    /**
     * Hàm khởi tạo sự kiện tìm kiếm
     * BTHOANG
     */
    searchFilter(){
        let me = this;
        $("#EmployeeFilter").find("[Filter = 'search']").on("keypress",function(event){
            if(event.key == "Enter"){
                me.search = $(this).val();
                console.log(me.search);
                me.reloadData();
            }
        });
    }

    /**
     * Hàm khởi tạo sự kiện bộ lọc vị trí
     * BTHOANG
     */
    positionFilter(){
        let me = this;
        $("#EmployeeFilter").find("[Filter = 'position']").on('change', function(){
            me.positionID = $(this).val();

            me.reloadData();
        });
    }

    /**
     * Hàm khởi tạo sự kiện bộ lọc phòng ban
     * BTHOANG
     */
    departmentFilter(){
        let me = this;
        $("#EmployeeFilter").find("[Filter = 'department']").on('change', function(){
            me.departmentID = $(this).val();

            me.reloadData();
        });
    }

    /**
     * Hàm khởi tạo sự kiện cho navigation
     * BTHOANG
     */
    initEventsNavigation(){
        let me = this;

        me.pageSize = $("input[SetField = 'pageSize']").val();
        
        $("input[SetField = 'pageSize']").on("keypress",function(event){
            if(event.key == 'Enter'){
                let value = $("input[SetField = 'pageSize']").val();
                if(20 > value){
                    value = 20;
                }
                if(value > 200){
                    value = 200;
                }
                $("input[SetField = 'pageSize']").val(value);

                me.pageSize = value;
                me.pageNumber = 1;
                me.reloadData();
            }
        });
    }

    /**
     * Hàm lấy page number
     * BTHOANG
     */
    getPageNumber(){
        let me = this;

        me.setGreenForNavigation();
        
        me.setNavigationBtn();

        me.setNextPrevNavigationBtn();
    }

    /**
     * Hàm nhận event click nút prev và next
     * BTHOANG
     */
    setNextPrevNavigationBtn(){
        let me = this;

        $(".navigations").find("[NavigationField]").off("click");
        $(".navigations").find("[NavigationField]").on("click", function(){
            let field = $(this).attr("NavigationField");

            // Gọi đến hàm động
            if(me[field] && typeof(me[field]) == "function"){
                me[field]();
            }
        });
    }

    /**
     * Hàm trở về trang đầu tiên
     * BTHOANG
     */
    doublePrev(){
        let me = this;

        me.pageNumber = 1;
        me.reloadData();
    }

    /**
     * Hàm trở về trang trước
     * BTHOANG
     */
    prev(){
        let me = this;
        
        me.pageNumber--;
        me.reloadData();
    }

    /**
     * Hàm đi tới trang cuối cùng
     * BTHOANG
     */
    doubleNext(){
        let me = this;

        me.pageNumber = me.pageTotalNumber;
        me.reloadData();
    }

    /**
     * Hàm next trang sau
     * BTHOANG
     */
    next(){
        let me = this;
        me.pageNumber++;
        me.reloadData();
    }



    /**
     * Hàm nhận event click trực tiếp nút ở navigation
     * BTHOANG
     */
    setNavigationBtn(){
        let me = this;

        $(".navigations").find("[PageNumber]").off('click');
        $(".navigations").find("[PageNumber]").on('click', function(){
            $('.navigations').find(".green-row").removeClass('green-row');
            $(this).addClass('green-row');
            me.pageNumber = $(this).attr("PageNumber");

            me.reloadData();
        });
    }

    /**
     * Hàm tự động gắn nền xanh cho trang navigation
     * BTHOANG
     */
    setGreenForNavigation(){
        let me = this;

        $(".navigations").find("[PageNumber]").each(function(){
            if($(this).attr("PageNumber") == me.pageNumber){
                $(this).addClass('green-row');
            }
        });
    }

    /**
     * Hàm Render phân trang
     * BTHOANG
     */
    renderNavigation(totalCount){
        let me = this,
            navigationPages = $(".navigation-pages");

        me.pageTotalNumber = parseInt(totalCount/me.pageSize);

        if((totalCount%me.pageSize) > 0){
            me.pageTotalNumber++;
        }
        navigationPages.html("");
        for (let i = 1; i <= me.pageTotalNumber; i++) {
            let page = $("<div></div>");
            page.attr("PageNumber", i);
            page.addClass('page');
            page.text(i);
            navigationPages.append(page);
        }

        me.getPageNumber();
    }

    /**
     * Hàm lấy tổng số bản ghi
     * BTHOANG
     */
    getTotalCount(totalCount){
        let me = this;

        let fisrtData = me.pageSize*(me.pageNumber - 1) + 1,
            finalData = me.pageSize*me.pageNumber;

        if(finalData > totalCount){
            finalData = totalCount;
        }
        let pageInfo = fisrtData + "-" + finalData + "/"; 

        $("span[SetField = 'pageInfo']").text(pageInfo);
        $("span[SetField = 'totalCount']").text(totalCount);

        me.renderNavigation(totalCount);
    }

    /**
     * Hàm khởi tạo sự kiện cho toolbar
     * BTHOANG
     */
    initEventsToolbar(){
        let me = this,
            toolbarId = me.grid.attr("Toolbar");

        // khởi tạo các sự kiện cho các button trong toolbar
        $(`#${toolbarId} [CommandType]`).off('click');
        $(`#${toolbarId} [CommandType]`).on('click', function(){
            let commandType = $(this).attr('CommandType');

            // gọi đến hàm động
            if(me[commandType] && typeof me[commandType] == "function"){
                me[commandType]();
            }
        });
    }

    /**
     * Hàm sự kiện click button add
     * BTHOANG
     */
    add(){
        let me = this,
            param = {
                parent: me,
                formMode: Enumeration.FormMode.Add,
            };
        
        // Lấy mã nhân viên mới
        me.getNewEmployeeCode(param);
    }

    /**
     * Hàm khởi tạo nhân bản
     */
    replication(){
        let me = this,
            param = {
                parent: me,
                formMode: Enumeration.FormMode.Replication
            },
            selectedEmployeeId = me.grid.find(".green-row").attr("EmployeeId");

        me.employeeData.filter(function(item){
            if(item.employeeID == selectedEmployeeId){
                param.employee = item;
            }
        });
        
        // Lấy mã nhân viên mới
        me.getNewEmployeeCode(param);
    }

    /**
     * Hàm khởi tạo click refresh
     * BTHOANG
     */
    refresh(){
        let me = this;

        // Đặt lại điều kiện lọc
        // me.search = "";
        // me.departmentID = "";
        // me.positionID = "";

        // Lấy lại dữ liệu của position vs department
        // me.getDepartmentData();
        // me.getPositionData();

        // Đặt lại searchbox
        // $("#EmployeeFilter").find("[Filter = 'search']").val("");

        CommonFn.toast('toast-messenger', Resource.Toast.info, 'Đang tải lại dữ liệu, vui lòng đợi!');
        // Lấy lại dữ liệu của employee
        me.reloadData();
    }

    /**
     * Hàm khởi tạo click edit button
     * BTHOANG
     */
    edit(){
        let me = this,
            param = {
                parent: me,
                formMode: Enumeration.FormMode.Edit
            },
            selectedEmployeeId = me.grid.find(".green-row").attr("EmployeeId");

        me.employeeData.filter(function(item){
            if(item.employeeID == selectedEmployeeId){
                param.employee = item;
            }
        });
        if(me.formDetail){
            me.formDetail.open(param);
        }
    }   

    /**
     * Hàm khởi tạo click delete button
     * BTHOANG
     */
    delete(){
        let me = this,
            param = {
                parent: me,
                formMode: Enumeration.FormMode.Delete,
            },
            selectedEmployeeId =  me.grid.find(".green-row").attr("EmployeeId");

        me.employeeData.filter(function(item){
            if(item.employeeID == selectedEmployeeId){
                param.employee = item;
            }
        });

        if(me.confirmForm){
            me.confirmForm.open(param);
        }
    }

    /**
     * Khởi tạo form detail
     * BTHOANG
     */
    initFormDetail(){
        let me = this;

        // khởi tạo đối tượng form detail
        me.formDetail = new EmployeeDetail("EmployeeDetail");
    }

    /**
     * Khởi tạo confirm form
     * BTHOANG
     */
    initConfirmForm(){
        let me = this;

        // khởi tạo đối tượng confirm form
        me.confirmForm = new EmployeeDetail("ConfirmForm");
    }

    /**
     * Hàm khởi tạo sự kiện cho table
     * BTHOANG
     */
    initEventsTable(){
        let me = this;

        me.grid.off("click", ".row");
        me.grid.on("click", ".row", function(){
            me.grid.find(".green-row").removeClass("green-row");

            $(this).addClass("green-row");
            $(".delete-btn").removeAttr('disabled');
            $(".replication-btn").removeAttr('disabled');
        });

        me.grid.off("dblclick", ".row");
        me.grid.on("dblclick", ".row", function(){
            me.edit();
        });
    }

    /**
     * Hàm lấy cấu hình các cột
     * BTHOANG
     */
    getColumnConfig(){
        let me = this,
            columnDefault = {
                FieldName : "",
                DataType : "String",
                EnumName: "",
                Text: "",
                ClassName: ""
            },
            columns = [];
        
        // Duyêt từng cột để lấy cấu hình
        me.grid.find(".col").each(function(){
            let column = {...columnDefault},
                that = $(this);
            
            Object.keys(columnDefault).filter(function(propName){
                let value = that.attr(propName);

                if(value){
                    column[propName] = value;
                }

                column.Text = that.text();
            });

            columns.push(column);
        });

        return columns;
    }

    /**
     * Hàm dùng để lấy dữ liệu cho trang
     * BTHOANG
     */
    getEmployeeData() {
        let me = this;
        if(me.search == null){
            me.search = "";
        }
        if(me.positionID == null){
            me.positionID = "";
        }
        if(me.departmentID == null){
            me.departmentID = "";
        }
        let url = `${Resource.APIs.Employees}?search=${me.search}&positionID=${me.positionID}&departmentID=${me.departmentID}&pageSize=${me.pageSize}&pageNumber=${me.pageNumber}`;
        console.log(url);
        CommonFn.Ajax(url, Resource.Method.Get, {}, function (response) {
            if (response) {
                me.loadData(response.data);
                me.getTotalCount(response.totalCount);
            } else {
                console.log("Có lỗi khi lấy dữ liệu từ server");
            }
        }, function(error){
            CommonFn.toast('toast-messenger', Resource.Toast.error, `Lỗi: '${error}' khi lấy dữ liệu nhân viên. Vui lòng liên hệ để được sửa chữa.`);
        });
    }

    /**
     * Hàm dùng để load dữ liệu
     * BTHOANG
     */
    loadData(data) {
        let me = this;
        if (data) {
            //Render dữ liệu cho grid
            me.renderGrid(data);

            me.employeeData = data;
        }

        CommonFn.toast('toast-messenger', Resource.Toast.success, 'Đã tải xong dữ liệu của nhân viên.');
    }

    /**
     * Render dữ liệu cho grid
     * BTHOANG
     */
    renderGrid(data) {
        let me = this,
            table = $("<div></div>"),
            tableHeader = me.renderHeader(),
            tableBody = me.renderBody(data);

        table.addClass("table");
        table.append(tableHeader);
        table.append(tableBody);

        me.grid.html(table);
    }

    /**
     * Render header
     * BTHOANG
     */
    renderHeader() {
        let me = this,
            thead = $("<div></div>");

        // Duyệt từng cột để vẽ header
        me.columnConfig.filter(function(column){
            let text = column.Text,
                className =column.ClassName,
                th = $("<div></div>");

            th.text(text);
            th.addClass("col");
            th.addClass(className);

            thead.append(th);
        })
        thead.addClass("table-header");

        return thead;
    }

    renderBody(data) {
        let me = this,
            tbody = $("<div></div>"),
            tcontent = $("<div></div>");

        if (data) {
            data.filter((item)=>{
                let row = $("<div></div>");
                // gán employeeId vào hàng
                row.attr("EmployeeId", item.employeeID);
                // Duyệt từng cột để vẽ body
                me.columnConfig.filter(function(column){
                    let fieldName = column.FieldName,
                        dataType = column.DataType,
                        td = $("<div></div>"),
                        value = me.getValueCell(item, fieldName, dataType),
                        className = column.ClassName;

                    td.text(value);
                    td.addClass("col");
                    td.addClass(className);

                    row.append(td);
                    row.addClass("row");
                });

                tcontent.append(row);
                tcontent.addClass("table-content");
            });
            tbody.append(tcontent);
            tbody.addClass("table-body");
        }

        return tbody;
    }

    /**
     * Lấy giá trị ô
     * item 
     * fieldName
     * dataType
     * 
     * BTHOANG
     */
    getValueCell(item, fieldName, dataType) {
        let me = this,
            value = item[fieldName];

        switch (dataType) {
            case Resource.DataTypeColumn.Number:
                value = CommonFn.formatMoney(parseInt(value));
                break;
            case Resource.DataTypeColumn.Date:
                value = CommonFn.formatDateOutput(value);
                break;
            case Resource.DataTypeColumn.Enum:
                switch (fieldName){
                    case "gender":
                        value = me.setGender(value);
                        break;
                    case "workStatus":
                        value = me.setWorkStatus(value);
                        break;
                }
                break;
        }

        return value;
    }

    /**
     * Hàm để set giá trị cho cột workStatus
     * BTHOANG
     */
    setWorkStatus(workStatus){
        let me = this;

        switch (workStatus) {
            case Enumeration.WorkStatus.NotWork:
                workStatus = Resource.WorkStatus.NotWork
                break;
        
            case Enumeration.WorkStatus.CurrentlyWorking:
                workStatus = Resource.WorkStatus.CurrentlyWorking
                break;

            case Enumeration.WorkStatus.StopWork:
                workStatus = Resource.WorkStatus.StopWork
                break;

            case Enumeration.WorkStatus.Retired:
                workStatus = Resource.WorkStatus.Retired
                break;
        }

        return workStatus;
    }

    /**
     * Hàm để set giá trị cho cột giới tính
     * BTHOANG
     */
    setGender(gender){
        let me = this;

        switch (gender) {
            case Enumeration.Gender.Female:
                gender = Resource.Gender.Female;
                break;
            case Enumeration.Gender.Male:
                gender = Resource.Gender.Male;
                break;
            case Enumeration.Gender.Other:
                gender = Resource.Gender.Other;
                break;
        }

        return gender;
    }

    /**
     * Hàm dùng để lấy class format cho từng kiểu dữ liệu
     * BTHOANG
     */
    getClassFormat(dataType) {
        let className = '';

        switch (dataType) {
            case Resource.DataTypeColumn.Number:
                className = "align-right";
                break;
            case Resource.DataTypeColumn.Date:
                className = "align-center";
                break;
        }

        return className;
    }
}

// Khởi tạo 1 biến cho trang nhân viên
var employeePage = new EmployeePage("gridEmployee");