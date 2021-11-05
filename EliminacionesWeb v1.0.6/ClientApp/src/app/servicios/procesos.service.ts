//Angular - rxjs
import { Injectable } from '@angular/core';
import { Component, Inject } from '@angular/core';
import { debug } from 'util';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
declare var $: any;


//Modelo
import { IUsuarios } from 'src/app/Model/Usuarios';
import { IGrupos } from 'src/app/Model/Grupos';
import { IProceso } from 'src/app/Model/Proceso';
import { IEmpresas } from 'src/app/Model/Empresas';
import { IMoneda } from 'src/app/Model/Moneda';
import { IRubros } from 'src/app/Model/Rubros';
import { ImpPlanillas } from 'src/app/Model/ImpPlanillas';
import { IAjustes } from 'src/app/Model/Ajustes';
import { ICalcular } from 'src/app/Model/Calcular';
import { IReportes } from 'src/app/Model/Reportes';
import { Itxt } from 'src/app/Model/Txt';
import { IGrupoEmpresa } from 'src/app/Model/Grupoempresa';
import { IHistorial } from '../Model/Historial';
import { IUsuariosGet } from '../Model/UsuariosGet';
import { PlanillasCargadas } from '../Model/PlanillasCargadas';
import { Impsubrubros } from 'src/app/Model/Subrubros';
import { IRelacionesRubros } from 'src/app/Model/RelacionesRubros';
import { IRubrocabecera } from 'src/app/Model/Rubrocabecera';
import { IGrandesRubros } from 'src/app/Model/Grandesrubros';
import { IRelacionesRubrosADD } from 'src/app/Model/RelacionesRubrosADD';


//Options
const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })

};


@Injectable({
    providedIn: 'root'
})

export class ProcesosService {
    myurl;

    // Servidor desarrollo \\Dco16liis51:9081\
    public data;
    public id: any;
    public myFecha;
    public myFechaimp;
    public myGrupoIdimp;
    public secCod;
    public logo: boolean = false;
    public fecaju;
    public fecreport;
    public empgrupo;
    public gruprowData;
    public grupoempresa;
    public empaju;
    public permitido: boolean = false;
    public fecajuShowModal;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.myurl = baseUrl;
    }


    postgrupoempresa(data) {
        return this.http.post<IGrupoEmpresa>(this.myurl + 'Eliminaciones/GrupoEmpresas', data, httpOptions);
    }

    deletegrupoempresa(data) {
        return this.http.post<IGrupoEmpresa>(this.myurl + 'Eliminaciones/GrupoEmpresas/DeleteGrupoEmpresas', data, httpOptions);
    }


    //Metodos Grupos AD
    //getAd(): Observable<IuserAD> {

    //    //this.legajo="l0694401"
    //    // return this.http.get<IuserAD>(this.myurl + 'Eliminaciones/Seguridad/GetUsuarioAD?Legajo='+this.legajo);
    //    return this.http.get<IuserAD>(this.myurl + 'Eliminaciones/Seguridad/GetUsuarioAD');
    //}

    getacceso() {
        console.log('GetUsuarioAD');
        var xhr = new XMLHttpRequest();
        xhr.open("GET", this.myurl + 'Eliminaciones/Seguridad/GetUsuarioAD', false);
        xhr.onload = function (e) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    //console.log(xhr.responseText);
                    sessionStorage.setItem('userlogin', xhr.responseText);
                    return;
                } else {
                    console.error(xhr.responseText);
                    sessionStorage.setItem('userlogin', null);
                    return;
                }
            }
        };
        xhr.onerror = function (e) {
            console.error(xhr.responseText);
            sessionStorage.setItem('userlogin', null);
            return;
        };
        xhr.send(null);



        return






    }

    //Metodos para Ajustes

    GetEmpresasByGrupoAjuste(): Observable<IEmpresas[]> {
        return this.http.get<IEmpresas[]>(this.myurl + 'Eliminaciones/GrupoEmpresas/GetEmpresasByGrupo?Grupo_Id=' + this.empaju + '&Sec_Codigo=' + this.secCod);
    }


    postAju(data) {

        return this.http.post<IAjustes>(this.myurl + 'Eliminaciones/Ajustes', data, httpOptions);
    }

    getAju(): Observable<IAjustes[]> {


        return this.http.get<IAjustes[]>(this.myurl + 'Eliminaciones/Ajustes/GetAjustesByPeriodo?Periodo=' + this.fecaju + '&Sec_Codigo=' + this.secCod);
    }

    deleteAju(data) {

        return this.http.post<IAjustes>(this.myurl + 'Eliminaciones/Ajustes/DeleteAjustes', data, httpOptions);

    }

    putAju(data) {
        return this.http.put<IAjustes>(this.myurl + 'Eliminaciones/Ajustes', data, httpOptions);
    }




    //Metodos para importacion de planillas

    getimpPlanillas(): Observable<ImpPlanillas[]> {

        // this.myFechaimp='072020'
        return this.http.get<ImpPlanillas[]>(this.myurl + 'Eliminaciones/ImportPlanillas/ByPeriodo?Periodo=' + this.myFechaimp + '&Sec_Codigo=' + this.secCod);
    }

    getPlanillasCargadas(): Observable<PlanillasCargadas[]> {

        return this.http.get<PlanillasCargadas[]>(this.myurl + 'Eliminaciones/ImportPlanillas/GetPlanillasCargadas?Periodo=' + this.myFechaimp + '&GrupoId=' + this.myGrupoIdimp + '&Sec_Codigo=' + this.secCod);
    }

    //Metodos para Rubros
    getRubros(): Observable<IRubros[]> {

        return this.http.get<IRubros[]>(this.myurl + 'Eliminaciones/Rubros');
    }

    getrubroscabecera(): Observable<IRubrocabecera[]> {

        return this.http.get<IRubrocabecera[]>(this.myurl + 'Eliminaciones/Rubros/GetRubrosCabecera');
    }

    getgrandesrubros(): Observable<IGrandesRubros[]> {

        return this.http.get<IGrandesRubros[]>(this.myurl + 'Eliminaciones/Rubros/GetGrandesRubros');
    }



    //Metodos para SubRubros
    getsubrubros(): Observable<Impsubrubros[]> {

        return this.http.get<Impsubrubros[]>(this.myurl + 'Eliminaciones/Rubros/GetSubRubros');
    }

    postSubrubro(data) {
        return this.http.post<Impsubrubros>(this.myurl + 'Eliminaciones/Rubros/CreateSubRubro', data, httpOptions);
    }


    deleteSubrubro(data) {
        return this.http.delete<Impsubrubros>(this.myurl + 'Eliminaciones/Rubros/DeleteSubRubros?Subrub_Id=' + data, httpOptions);
    }

    putSubrubro(data) {
        return this.http.put<Impsubrubros>(this.myurl + 'Eliminaciones/Rubros/UpdateSubRubro?Subrub_Id=' + data.subrubId, data, httpOptions);
    }

    //Metodos para Relacion Rubros

    getrelacionRubros(): Observable<IRelacionesRubros[]> {

        return this.http.get<IRelacionesRubros[]>(this.myurl + 'Eliminaciones/Rubros/GetRelacionesRubros');
    }

    postrelacionRubros(data) {
        return this.http.post<IRelacionesRubrosADD>(this.myurl + 'Eliminaciones/Rubros/CreateRelacionRubro', data, httpOptions);
    }


    deleterelacionRubroso(data) {
        return this.http.post<IRelacionesRubrosADD>(this.myurl + 'Eliminaciones/Rubros/DeleteRelacionRubros', data, httpOptions);
    }

    //putrelacionRubros(data) {
    //  return this.http.put<IAjustes>(this.myurl + 'Eliminaciones/Ajustes', data, httpOptions);
    //}


    //Metodos para Moneda 
    getMoneda(): Observable<IMoneda[]> {

        return this.http.get<IMoneda[]>(this.myurl + 'Eliminaciones/Monedas', httpOptions);
    }


    //Metodos para Empresas

    GetEmpresasByGrupo(): Observable<IEmpresas[]> {
        return this.http.get<IEmpresas[]>(this.myurl + 'Eliminaciones/GrupoEmpresas/GetEmpresasByGrupo?Grupo_Id=' + this.empgrupo + '&Sec_Codigo=' + this.secCod);
    }

    postEmp(data) {

        return this.http.post<IEmpresas>(this.myurl + 'Eliminaciones/Empresas', data, httpOptions);
    }

    getEmp(): Observable<IEmpresas[]> {

        return this.http.get<IEmpresas[]>(this.myurl + 'Eliminaciones/Empresas/' + this.secCod);
    }

    getEmpbyperiodo(): Observable<IEmpresas[]> {

        this.myFecha = '072020'
        return this.http.get<IEmpresas[]>(this.myurl + 'Eliminaciones/Empresas/GetEmpresasByPeriodo?Periodo=' + this.myFecha + '&Sec_Codigo=' + this.secCod);
    }



    deleteEmp(id: string) {

        return this.http.delete<IEmpresas>(this.myurl + 'Eliminaciones/Empresas?Emp_Codigo=' + id + '&Sec_Codigo=' + this.secCod, httpOptions);
    }



    putEmp(obj) {

        return this.http.put<IEmpresas>(this.myurl + 'Eliminaciones/Empresas/' + obj.empCodigo, obj, httpOptions);
    }




    //Metodos para Procesos
    // getPeriodo1(): Observable<IProceso[]>{

    //   return this.http.get<IProceso[]>(this.myurl + '/api/proceso');
    //  }

    postCalcular(data) {

        console.log(data)
        //this.myFecha='072020'
        return this.http.post<ICalcular>(this.myurl + 'Eliminaciones/Procesos/Calcular?Periodo=' + data.periodo + '&GrupoId=' + data.grupo + '&Sec_Codigo=' + this.secCod, httpOptions);
    }

    getPeriodo(data) {

        return this.http.get<IProceso[]>(this.myurl + 'Eliminaciones/DatosPlanillas/GetDatosPlanillaByPeriodo?Periodo=' + data.periodo + "&GrupoId=" + data.grupo + '&Sec_Codigo=' + this.secCod, httpOptions);
        // Eliminaciones/DatosPlanillas/GetDatosPlanillaByPeriodo?Periodo=072020&GrupoId=1&Sec_Codigo=1
    }



    // Metodos de Usuarios
    postUsuers(data) {

        return this.http.post<IUsuarios>(this.myurl + 'Eliminaciones/Usuarios', data, httpOptions);
    }
    //ANG secCod
    getUsers(): Observable<IUsuariosGet[]> {

        return this.http.get<IUsuariosGet[]>(this.myurl + 'Eliminaciones/Usuarios/' + this.secCod);
    }

    deleteUsers(legajo: string) {

        return this.http.delete<IUsuarios>(this.myurl + 'Eliminaciones/Usuarios?Usu_Legajo=' + legajo + '&Sec_Codigo=' + this.secCod, httpOptions);
    }

    putUsers(obj) {

        return this.http.put<IUsuarios>(this.myurl + 'Eliminaciones/Usuarios/' + obj.usuLegajo, obj, httpOptions);
    }

    getPerfil() {
        return this.http.get<IUsuariosGet[]>(this.myurl + 'Eliminaciones/Usuarios/GetPerfiles');
    }


    // Metodos de Grupo
    getGrupo(): Observable<IGrupos[]> {

        return this.http.get<IGrupos[]>(this.myurl + 'Eliminaciones/Grupos/' + this.secCod);

    }
    postGrupo(data) {

        return this.http.post<IGrupos>(this.myurl + 'Eliminaciones/Grupos', data, httpOptions);
    }

    deleteGrupo(id: string) {
        return this.http.delete<IGrupos>(this.myurl + 'Eliminaciones/Grupos?Grupo_Id=' + id + '&Sec_Codigo=' + this.secCod, httpOptions);
    }

    putGrupo(obj) {

        return this.http.put<IGrupos>(this.myurl + 'Eliminaciones/Grupos/' + obj.grupoId, obj, httpOptions);
    }



    //Metodos para reportes

    GetEliminaciones(data): Observable<IReportes[]> {

        return this.http.get<IReportes[]>(this.myurl + 'Eliminaciones/Procesos/GetEliminaciones?Periodo=' + data.fecha + '&Sec_Codigo=' + this.secCod); //, httpOptions);
        ///Eliminaciones/Procesos/GetEliminaciones?Periodo=072020&GrupoId=1&Sec_Codigo

    }

    posttxt() {

        // https://localhost:44368/Eliminaciones/Procesos/GenerarTxt?Periodo=072020&Sec_Codigo=1
        return this.http.post<Itxt>(this.myurl + 'Eliminaciones/Procesos/GenerarTxt?Periodo=' + this.fecreport + '&Sec_Codigo=' + this.secCod, httpOptions);
    }


    GetGenerarExcel(data) {

        //return this.http.get<File[]>(this.myurl + 'Eliminaciones/Procesos/GenerarExcel?Periodo=' + data.fecha + '&Sec_Codigo=' + this.secCod, httpOptions);

        var url = this.myurl + 'Eliminaciones/Procesos/GenerarExcel?Periodo=' + data.fecha + '&Sec_Codigo=' + this.secCod;
      this.exportExcel(url, 'Eliminaciones');
      $('#excel').modal('toggle');
     
          
    }

    //GetGenerarExcel(data) {
    //    this.http.get('my-api-url', { responseType: 'blob' }).subscribe(blob => {
    //        saveAs(blob, 'SomeFileDownloadName.someExtensions', {
    //            type: 'text/plain;charset=windows-1252' // --> or whatever you need here
    //        });
    //    });
    //}


    exportExcel(url, name) {
        var filename: string
        var request = new XMLHttpRequest();
        request.open('GET', url, true);
        request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        request.responseType = 'blob';

        request.onload = function (e) {
            if (this.status === 200) {
                var blob = this.response;
                if (window.navigator.msSaveOrOpenBlob) {
                    window.navigator.msSaveBlob(blob, filename);
                }
                else {
                    var downloadLink = window.document.createElement('a');
                    var contentTypeHeader = request.getResponseHeader("Content-Type");
                    downloadLink.href = window.URL.createObjectURL(new Blob([blob], { type: contentTypeHeader }));
                    var d = new Date();

                    downloadLink.download = name + "_" + d.getFullYear() + (d.getMonth() + 1) + d.getDate() + ".xlsx";
                    document.body.appendChild(downloadLink);
                    downloadLink.click();
                    document.body.removeChild(downloadLink);
                }
            }
        };
        request.send();

    }

    //Metodos para Historial
    getHistorial(): Observable<IHistorial[]> {

        return this.http.get<IHistorial[]>(this.myurl + 'Eliminaciones/Historials/ ' + this.secCod);
    }


}
