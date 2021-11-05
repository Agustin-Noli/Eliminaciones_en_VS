import { Component, OnInit, ViewChild, Input, ɵConsole } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArrayName, FormArray } from '@angular/forms';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { AgGridAngular } from 'ag-grid-angular';
import { ActivatedRoute, Router } from '@angular/router';
import * as xJS from '../../../locale.en.js';
import { debug } from 'util';
//import { NgSelectModule, NgOption } from '@ng-select/ng-select';

@Component({
    selector: 'app-ajustes',
    templateUrl: './ajustes.component.html',
    styleUrls: ['./ajustes.component.scss']
})



export class AjustesComponent implements OnInit {


    rowSelection;
    defaultColDef;
    showModal: boolean = false;
    showModeleliminar: boolean = false;
    showModelmodificar: boolean = false;
    showMsjemp: boolean = false;
    showMsj: boolean = false;
    showconf: boolean = false;
    showMsjemp1: boolean = false;
    showMsjmod: boolean = false;
    showconfr: boolean = false;
    success: boolean = false;
    successm: boolean = false;
    successelim: boolean = false;
    successe: boolean = false;
    disabled: boolean = false;
    error: boolean = false;
    emp: any;
    rowData: any;
    rubros: any;
    id = ''; desc = ''; porc = ''; interco = ''; obj; datos; datosg;
    porcenjate = ""; descripcion = ""; interc = ""; msjelim = ""; msjelim1 = ""; title = "";
    empCodigo; rubDescripcion; empCodigoContraparte; fecaju; periodoaju;

    empresam;
    periodom;
    rubrom;
    contrapartem;
    saldom;
    saldopromediom;
    fecajuactual
    //prueba fecha proceso
    fecha: string;
    campofecha;
    camposelect = "Selecciona una opción";
    selectedRubro: any;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;


    columnDefs = [
        { headerName: 'Codigo', field: 'empCodigo', sortable: true, filter: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Empresa', field: 'empDescripcion', sortable: true, filter: true, resizable: true, suppressMovable: true },
        { headerName: 'Periodo', field: 'periodo', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Rubro', field: 'rubCodigo', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Descripcion', field: 'rubDescripcion', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Codigo Contraparte', field: 'empCodigoContraparte', sortable: true, resizable: true, suppressMovable: true, hide: true },
        { headerName: 'Contraparte', field: 'empDescripcionContraparte', sortable: true, resizable: true, filter: true, suppressMovable: true },
        { headerName: 'Saldo', field: 'ajuSaldo', sortable: true, resizable: true, suppressMovable: true, valueFormatter: x => numberToPointedString(x.value) },
        { headerName: 'Saldo Promedio', field: 'ajuSaldoPromedio', sortable: true, resizable: false, suppressMovable: false, valueFormatter: x => numberToPointedString(x.value) }



    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;

    // private http: HttpClient,
    myformA: FormGroup
    myformM: FormGroup
    constructor(private procesosServ: ProcesosService, private a: FormBuilder, private m: FormBuilder, private route: ActivatedRoute, private router: Router) {

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


        this.initFormA();

        this.myformM = this.m.group({
            empresam: ["", Validators.required],
            periodom: [this.procesosServ.fecaju, Validators.required],
            rubrom: [""],
            contrapartem: ["", Validators.required],
            saldom: ["", Validators.required],
            saldopromediom: [""]
        });

        this.defaultColDef = {
            flex: 1,
            minWidth: 100
        };

        this.rowSelection = 'single';
        this.convertirfecha();

    }

    ngOnInit() {

        this.procesosServ.getAju()
            .subscribe((data) => {
                this.rowData = ""

                if (data != null) {
                    this.rowData = data
                }
            }, err => {
                console.log(err);
            }
            );
        //modificar get emp
        this.procesosServ.GetEmpresasByGrupoAjuste()
            .subscribe((data) => {
                if (data != null) {
                    this.emp = data
                }
            }, err => {
                console.log(err);
            }
            );

        this.procesosServ.getRubros()
            .subscribe((data) => {
                if (data != null) {
                    this.rubros = data
                }
            }, err => {
                console.log(err);
            }
            );

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

            this.desc = this.obj.empDescripcion
            this.myformM.value.empresam = this.obj.empDescripcion

        }

    }
    convertirfecha() {

        let fecajuactual = this.procesosServ.fecaju.split(" ")[0].split("-").reverse().join("-");
        let datafecha = fecajuactual.replace("-", "")
        this.procesosServ.fecaju = datafecha
    }


    initFormA() {
        this.myformA = this.a.group({
            empresa: [null, Validators.required],
            periodo: [this.procesosServ.fecajuShowModal],
            rubro: [null, Validators.required],
            contraparte: ["", Validators.required],
            saldo: ["", Validators.required],
            saldopromedio: [""]

        });
    }

    showModel() {

        this.title = 'Agregar'
        this.error = false;
        this.disabled = false;
        this.showModal = true;
        this.showMsjemp = true;
        this.success = false;
    }


    showModele() {
        this.successe = false
        if (this.obj == undefined) {

            this.title = 'Atencion';
            this.showModeleliminar = true;
            this.showMsjemp = true;
            this.showMsjemp1 = false;
            this.msjelim = "Debe seleccionar un ajuste";
        } else {

            this.title = "Eliminar";
            this.showModeleliminar = true;
            this.showMsj = true;
            this.showconf = true;
            this.showMsjemp = false;
            this.showMsjemp1 = true;
            this.msjelim1 = "¿Desea eliminar el ajuste "
        }
    }

    enviaelim() {
        var data = {
            "empCodigo": this.obj.empCodigo,
            "periodo": this.obj.periodo,
            "rubCodigo": this.obj.rubCodigo,
            "empCodigoContraparte": this.obj.empCodigoContraparte,
            "ajuSaldo": this.obj.ajuSaldo,
            "ajuSaldoPromedio": this.obj.ajuSaldoPromedio,
            "SecCodigo": this.procesosServ.secCod
        }


        this.procesosServ.deleteAju(data).subscribe((res) => {
            if (res == res) {

                this.procesosServ.getAju()
                    .subscribe(res => {
                        this.rowData = res

                        this.successe = true
                        this.showconf = false;
                        setTimeout(() => {
                            this.showModeleliminar = false;

                        }, 2000);
                        this.obj = undefined

                    }, err => {
                        console.log(err);

                    });
            }

        })

    }



    cerrar() {
        this.showModal = false;
        this.myformA.reset();
        this.initFormA()

    }
    enviar() {
        debugger
        this.fecaju = this.procesosServ.fecaju
        this.periodoaju = this.fecaju.split(" ")[0].split("-").reverse().join("-");
        this.fecaju = this.periodoaju.replace("-", "")

        let fecaju = (this.myformM.value.saldopromediom == null) ? this.fecaju : this.fecaju;
        let ajusaldopromedio = (this.myformA.value.saldopromedio == "") ? 0 : this.myformA.value.saldopromedio
        var data = {
            "empCodigo": this.myformA.value.empresa,
            "periodo": fecaju, //072020
            "rubCodigo": this.myformA.value.rubro,
            "empCodigoContraparte": this.myformA.value.contraparte.empCodigo,
            "ajuSaldo": this.myformA.value.saldo,
            "ajuSaldoPromedio": ajusaldopromedio,
            "SecCodigo": this.procesosServ.secCod
        }


        this.procesosServ.postAju(data).subscribe((res) => {
            if (res == res) {
                this.procesosServ.getAju()
                    .subscribe(res => {
                        this.rowData = res;
                        this.success = true;
                        this.disabled = true;
                        setTimeout(() => {
                            //   this.showModal = false;
                            this.success = false;
                            this.disabled = false;
                            this.myformA.controls.saldo.reset();
                            this.myformA.controls.saldopromedio.reset();
                        }, 1500);
                        //    this.myformA.reset();

                        //this.initFormA()

                    }, err => {
                        console.log(err);
                    });
            }
        }, err => {

            if (err.status === 409) {
                this.error = true
                this.disabled = true;
                setTimeout(() => {
                    this.error = false;
                    this.disabled = false;
                    this.myformA.controls.saldo.reset();
                    this.myformA.controls.saldopromedio.reset();
                }, 1500);
            }
        })

        this.myformA.value.periodo = this.procesosServ.fecajuShowModal
        this.camposelect = "Selecciona una opción";
    }
    showModelm() {

        this.successm = false;

        if (this.obj == undefined) {
            this.title = 'Atencion';
            this.showModelmodificar = true;
            this.showMsjemp = true;
            this.showMsjmod = false;
            this.showconfr = false;
            this.msjelim = "Debe seleccionar un ajuste"
        } else {
            setTimeout(() => {
                this.empresam = this.obj.empDescripcion
                this.rubrom = this.obj.rubCodigo  + "- " + this.obj.rubDescripcion
                this.contrapartem = this.obj.empDescripcionContraparte
                this.saldom = this.obj.ajuSaldo
                this.saldopromediom = this.obj.ajuSaldoPromedio
            }, 100);
            this.title = "Modificar";
            this.showModelmodificar = true;
            this.showMsjmod = true;
            this.showMsjemp = false;
            this.showconfr = true;
        }
    }



    enviamod() {
        let codigoemp = ((this.myformM.value.empresam == null || this.myformM.value.empresam == "") ? this.obj.empCodigo : this.myformM.value.empresam.empCodigo);
        let codigorub = ((this.myformM.value.rubrom == null || this.myformM.value.rubrom == "") ? this.obj.rubCodigo : this.myformM.value.rubrom.rubCodigo);
        let codigoempcontraparte = ((this.myformM.value.contrapartem == null || this.myformM.value.contrapartem == "") ? this.obj.empCodigoContraparte : this.myformM.value.contrapartem.empCodigo);
        let saldoaju = ((this.myformM.value.saldom == null || this.myformM.value.saldom == "") ? this.obj.ajuSaldo : this.myformM.value.saldom);
        let saldoajupromedio = ((this.myformM.value.saldopromediom == null || this.myformM.value.saldopromediom == "") ? this.obj.ajuSaldoPromedio : this.myformM.value.saldopromediom);



        var data = {
            "empCodigo": codigoemp,
            "periodo": this.obj.periodo,
            "rubCodigo": codigorub,
            "empCodigoContraparte": codigoempcontraparte,
            "ajuSaldo": saldoaju,
            "ajuSaldoPromedio": saldoajupromedio,
            "SecCodigo": this.procesosServ.secCod
        }

        this.procesosServ.putAju(data).subscribe((res) => {
            if (res == res) {
                this.procesosServ.getAju()
                    .subscribe(res => {
                        this.rowData = res
                        this.successm = true;
                        this.showconfr = false;
                        setTimeout(() => {
                            this.obj = undefined
                            this.showModelmodificar = false;
                        }, 2000);

                    }, err => {
                        console.log(err);

                    });
            }

        })

    }

   



}
function numberToPointedString(number) {
    let result = number.toLocaleString('es-ar', {
        style: 'currency',
        currency: 'ARS',
        minimumFractionDigits: 2
    });

    return result;

}

