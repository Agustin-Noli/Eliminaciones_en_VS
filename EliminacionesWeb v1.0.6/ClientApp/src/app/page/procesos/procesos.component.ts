import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArrayName, FormArray } from '@angular/forms';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { AgGridAngular } from 'ag-grid-angular';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';
import { debug } from 'util';

import * as bootstrap from "bootstrap";

@Component({
    selector: 'app-procesos',
    templateUrl: './procesos.component.html',
    styleUrls: ['./procesos.component.scss'],
    providers: [DatePipe]
})

export class ProcesosComponent implements OnInit {
    rowSelection;
    defaultColDef
    showModal: boolean = false;
    showModeleliminar: boolean = false;
    showModelmodificar: boolean = false;
    showMsjemp: boolean = false;
    showMsj: boolean = false;
    showconf: boolean = false;
    showMsjemp1: boolean = false;
    showMsjmod: boolean = false;
    showconfr: boolean = false;
    showCalcular: boolean = false;
    alertcalculo: boolean = false;
    errorcalculo: boolean = false;
    valid: boolean = false;
    defaultrgup = "Mensual";
    id = ''; idgrupo;
    desc = '';
    porc = '';
    interco = ''; obj;
    porcenjate = ""; descripcion = ""; interc = ""; msjelim = ""; msjelim1 = ""; title = "";
    txt; calculo;
    fecha;
    rfecha;
    fec: any;
    fecperiodoactual;
    newmyModel;
    date = new Date();
    prueba = []; grupos; idgruponom;
    //grupoValue;

    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;


    columnDefs = [
        { headerName: 'Codigo Empresa', field: 'empCodigo', sortable: true, filter: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Empresa', field: 'empDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Periodo', field: 'periodo', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Codigo Rubro', field: 'rubCodigo', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Rubro', field: 'rubDescripcion', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Concepto', field: 'concepto', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Codigo Contraparte', field: 'empCodigoContraparte', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Contraparte', field: 'empDescripcionContraparte', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Codigo Moneda', field: 'monCodigo', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Moneda', field: 'monDescripcion', sortable: true, resizable: true, suppressMovable: true },
        { headerName: 'Saldo', field: 'saldo', sortable: true, resizable: true, suppressMovable: true, valueFormatter: x => numberToPointedString(x.value) },
        { headerName: 'Saldo Promedio', field: 'saldoPromedio', sortable: true, resizable: true, suppressMovable: true, valueFormatter: x => numberToPointedString(x.value) },
        { headerName: 'Ind', field: 'ind', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Pond', field: 'pond', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Exposicion', field: 'exposicion', sortable: true, resizable: true, suppressMovable: true, hide: true },

    ];

    public localeText = xJS.AG_GRID_LOCALE_EN;

    rowData: any;
    myform: FormGroup;

    constructor(private mf: FormBuilder, private procesosServ: ProcesosService, private datePipe: DatePipe, private route: ActivatedRoute, private router: Router) {
        var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));

        if (UserAD == undefined || UserAD == null) {

            this.procesosServ.getacceso()
            UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
            if (UserAD == null) {
                this.router.navigate(['']);
            }
        }
        if (route.snapshot.url[0] == undefined) {
            this.procesosServ.logo = true;
        } else {
            this.procesosServ.logo = false;
        }


        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };

        if (this.procesosServ.fecaju == undefined)
            this.fecha = this.datePipe.transform(this.date, 'yyyy-MM');
        else
            this.fecha = this.procesosServ.fecaju.toString().substring(2, 6) + '-' + this.procesosServ.fecaju.toString().substring(0, 2);


        this.getgrupos();

        this.myform = this.mf.group({
            inpfecha: [this.fecha, Validators.required],
            grupo: ["", Validators.required]
        })

        this.rowSelection = 'single';

        this.procesosServ.fecajuShowModal = this.fecha
        this.rfecha = this.fecha.split(" ")[0].split("-").reverse().join("-");
        this.fec = this.rfecha.replace("-", "")
        this.procesosServ.myFecha = this.fec
    }

    ngOnInit() {

    }

    onSelectionChanged(e) {

        const selectedNodes = this.agGrid.api.getSelectedNodes();
        // const selectedData = selectedNodes.map( node => node.data );
        // const selectedDataStringPresentation = selectedData.map( node => node.make + ' ' + node.model).join(', ');
        this.obj = selectedNodes
        if (this.obj.length == 0) {
            this.obj = undefined;
        } else {
            this.obj = selectedNodes[0].data
        }
    }


    getgrupos() {
        this.procesosServ.getGrupo()
            .subscribe((data: any) => {
                if (data != null) {
                    this.grupos = data
                    //this.idgrupo = this.grupos[0].grupoId;
                    this.idgrupo = (this.procesosServ.empaju == undefined) ? this.grupos[0].grupoId : this.procesosServ.empaju;
                    //seteo el formulario con lo cargado anteriormente
                    this.myform.controls['grupo'].setValue(this.idgrupo);
                    this.procesosServ.empaju = this.idgrupo
                    this.periodoactual();
                }
            }, err => {
                console.log(err);
            }
            );

    }

    periodoactual() {
        var data = {
            "grupo": this.idgrupo,
            "periodo": this.procesosServ.myFecha
        }

        this.procesosServ.getPeriodo(data)
            .subscribe((data) => {
                if (data != null) {
                    this.rowData = data
                    this.valid = true
                } else {
                    this.rowData = ""
                    this.valid = false
                }
            }
                , err => {
                    if (err.status == 404) {
                        this.rowData = ""
                    }
                }

            );
    }

    fechaajuste() {
        this.procesosServ.fecaju = this.fecha
        this.procesosServ.fecajuShowModal = this.fecha
    }

    periodo() {

        this.fecha = this.myform.value.inpfecha
        this.fecperiodoactual = this.fecha.split(" ")[0].split("-").reverse().join("-");
        let datafecha = this.fecperiodoactual.replace("-", "")

        let grup = ((this.myform.value.grupo == null || this.myform.value.grupo == "") ? this.idgrupo : this.myform.value.grupo);

        this.grupos.forEach(item => {
            if (this.myform.value.grupo == item.grupoNombre) {
                //console.log(item.grupoId)
                grup = item.grupoId
                return
            }
        });
        this.procesosServ.empaju = grup
        var data = {
            "periodo": datafecha,
            "grupo": grup
        }

        this.procesosServ.getPeriodo(data)
            .subscribe(data => {
                if (data != null) {
                    this.rowData = data
                    this.valid = true
                } else {
                    this.rowData = ""
                    this.valid = false
                }
            }
                , err => {
                    if (err.status == 404) {
                        this.rowData = ""
                    }
                }
            );

    }

    ShowCalcular() {
        this.showCalcular = true;
        this.alertcalculo = false;

    }
    calcular() {
        // mandar solo grupoid
        this.fecha = this.myform.value.inpfecha
        this.fecperiodoactual = this.fecha.split(" ")[0].split("-").reverse().join("-");

        let datafecha = this.fecperiodoactual.replace("-", "")
        let grup = ((this.myform.value.grupo == null || this.myform.value.grupo == "") ? this.idgrupo : this.myform.value.grupo);

        if (this.myform.value.grupo != "") {
            this.grupos.forEach(item => {
                if (this.myform.value.grupo == item.grupoNombre) {
                    console.log(item.grupoId)
                    grup = item.grupoId
                    return
                }
            });
        }
        let data = {
            "periodo": datafecha,
            "grupo": grup
        }

        this.procesosServ.postCalcular(data)
            .subscribe(res => {
                if (res.mensaje === "OK") {
                    this.alertcalculo = true;
                    //setTimeout(() => {
                    //    this.showCalcular = false;
                    //}, 2000);
                }
                else {
                    this.errorcalculo = true;
                    //setTimeout(() => {
                    //    this.showCalcular = false;
                    //}, 2000);
                }
            }
                //err => {
                //    console.log(err)
                //}

            );




    }

}

function numberToPointedString(number) {
    if (number == null) {
        return number;
    } else {
        let result = number.toLocaleString('es-ar', {
            style: 'currency',
            currency: 'ARS',
            minimumFractionDigits: 2
        });

        return result;
    }


}


