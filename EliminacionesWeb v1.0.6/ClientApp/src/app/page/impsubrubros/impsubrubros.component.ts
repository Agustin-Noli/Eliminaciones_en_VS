import { Component, OnInit, ViewChild } from '@angular/core';
import { AgGridAngular } from 'ag-grid-angular';
import { HttpClient } from '@angular/common/http';
import { ProcesosService } from 'src/app/servicios/procesos.service'
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResolvedStaticSymbol } from '@angular/compiler';
import * as xJS from '../../../locale.en.js';

@Component({
    selector: 'app-impsubrubros',
    templateUrl: './impsubrubros.component.html',
    styleUrls: ['./impsubrubros.component.scss']
})
export class ImpsubrubrosComponent implements OnInit {
    showModal: boolean = false;
    showModale: boolean = false;
    showModalsubrubro: boolean = false;
    eliminar: boolean = false;
    default: boolean = false;
    confirmar: boolean = false;
    successelim: boolean = false;
    permitido: boolean = false;
    confirmarguardar: boolean = false;
    error: boolean = false;
    successm: boolean = false;

    showMsjmod: boolean = false;
    showMsjemp: boolean = false;
    showconf: boolean = false;
    defaultColDef;
    rowSelection;
    @ViewChild('agGrid', { static: false }) agGrid: AgGridAngular;
    title; mesajee;
    mesaje;
    success;
    id;
    msjemp;
    msjelim;
    subrubrom;
    elimina;
    // 
    columnDefs = [
        { headerName: 'Codigo', field: 'subrubId', sortable: true, filter: true, hide: true },
        { headerName: 'Sub-Rubro', field: 'subrubDescripcion', sortable: true, filter: true }
        //{headerName: 'SecCodigo', field: 'secCodigo', sortable: true, resizable: true,suppressMovable:true,hide:true}
    ];
    public localeText = xJS.AG_GRID_LOCALE_EN;
    obj;
    rowData: any;
    myform: FormGroup
    myformMod: FormGroup

    constructor(private http: HttpClient, private procesosServ: ProcesosService, private a: FormBuilder, private m: FormBuilder, private route: ActivatedRoute, private router: Router) {

        var UserAD = JSON.parse(sessionStorage.getItem('userlogin'));

        if (UserAD == undefined || UserAD == null) {

            this.procesosServ.getacceso()
            UserAD = JSON.parse(sessionStorage.getItem('userlogin'));
            if (UserAD == null) {
                this.router.navigate(['']);
            }
        }
        let permit = (UserAD.usuPerfil == "Operador") ? false : true;

        this.permitido = permit

        if (route.snapshot.url[0] == undefined) {
            this.procesosServ.logo = true;
        } else {
            this.procesosServ.logo = false;
        }



        this.formulariosubrubros();
        this.defaultColDef = {
            flex: 1,
            minWidth: 100,
        };
        this.rowSelection = 'single';

    }

    ngOnInit() {
        this.procesosServ.getsubrubros()
            .subscribe((res) => {

                if (res.length != 0) {
                    this.rowData = res
                } else {
                    this.rowData = ""
                }
            }, err => {
                console.log(err)
            }

            );


    }


    formulariosubrubros() {
        this.myform = this.a.group({
            subrubro: ["", Validators.required]

        });

        this.myformMod = this.m.group({
            subrubroMod: ["", Validators.required]
        });


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
            this.subrubrom = this.obj.subrubDescripcion
            this.msjemp = this.obj.subrubDescripcion
            //this.msjemp=this.obj.empDescripcion
            this.elimina = this.obj.subrubId

        }

    }

             
    showmodals() {
        this.confirmarguardar = false
        this.error = false
        this.formulariosubrubros()
        this.showModalsubrubro = true

    }

    showModelm() {
        this.showconf = false
        this.successm = false

        if (this.obj == undefined) {
            this.title = 'Atencion';
            this.eliminar = true
            this.showMsjemp = true;
            this.showMsjmod = false;
            this.mesaje = "Debe selecionar un Subrubro"

            this.showModal = true
        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.eliminar = true
                this.showMsjemp = true;
                this.mesaje = "Debe selecionar un Subrubro"
                this.showModal = true


            } else {
                setTimeout(() => {
                    this.subrubrom = this.obj.subrubDescripcion

                }, 100);
                this.title = "Modificar";
                this.showModal = true;
                this.showMsjmod = true;
                this.showMsjemp = false;
                this.showconf = true;
            }
        }

    }


    enviamod() {

        let subRubDesc = ((this.myformMod.value.subrubroMod == null || this.myformMod.value.subrubroMod == "") ? this.obj.subrubDescripcion : this.myformMod.value.subrubroMod);

        var data = {
            "subrubId": this.obj.subrubId,
            "subrubDescripcion": subRubDesc,
            "activo": "S"
        }

        this.procesosServ.putSubrubro(data).subscribe((id) => {
            if (id == id) {
                this.procesosServ.getsubrubros()
                    .subscribe(res => {
                        this.rowData = res
                        this.successm = true
                        setTimeout(() => {
                            this.obj = undefined
                            this.myformMod.reset()
                            this.showModal = false;
                        }, 1500);


                    }, err => {
                        console.log(err);

                    });
            }

        })

    }

    guardarsubrubro() {

        let data = {

            "SubrubDescripcion": this.myform.value.subrubro,
            "Activo": ""
        }
        this.procesosServ.postSubrubro(data).subscribe(res => {

            if (res == res) {
                this.procesosServ.getsubrubros()
                    .subscribe(res => {
                        this.rowData = res
                        this.success = true
                        this.confirmarguardar = true
                        setTimeout(() => {
                            this.success = false
                            this.showModalsubrubro = false
                        }, 1500);
                    }, err => {

                        if (err.status === 409) {
                            this.error = true;
                        }
                        console.log(err);

                    });
            }

        }, err => {

            if (err.status === 409) {
                this.error = true
                this.confirmarguardar = true
            }
            console.log(err);
        })



    }

    showmodale() {
        this.confirmar = false

        if (this.obj == undefined) {

            this.title = 'Atencion';
            this.eliminar = true
            this.default = false
            this.mesajee = "Debe selecionar un Subrubro"
            this.confirmar = true
            this.showModale = true
        } else {
            if (this.id == "") {
                this.title = 'Atencion';
                this.eliminar = true
                this.default = false
                this.mesajee = "Debe selecionar un Subrubro"
                this.showModale = true
                this.confirmar = true

            } else {
                this.title = 'Eliminar';
                this.eliminar = false
                this.default = true
                this.mesajee = "Desea eliminar el subrubro "
                this.showModale = true
            }
        }

    }
    confirmareliminacion() {


        this.procesosServ.deleteSubrubro(this.elimina).subscribe(res => {
            if (res == res) {
                this.procesosServ.getsubrubros()
                    .subscribe(res => {
                        this.rowData = res
                        this.successelim = true
                        this.confirmar = true
                        setTimeout(() => {
                            this.successelim = false
                            this.showModale = false
                        }, 1500);
                    }, err => {
                        console.log(err);

                    });
            }

        }), err => {
            console.log(err);
        }
    }


}
