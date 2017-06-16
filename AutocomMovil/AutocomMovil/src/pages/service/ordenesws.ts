import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

/*
  Generated class for the ordenesws provider.

  See https://angular.io/docs/ts/latest/guide/dependency-injection.html
  for more info on providers and Angular 2 DI.
*/
@Injectable()
export class ordenesws {

    static get parameters() {
        return [[Http]];
    }

    constructor(public http: Http) {
        
    }

    buscarOrden(vin) {
        var url = 'http://localhost/GoVirtualMCo/WsVDealer/WsConsultaOrden.asmx/getOrdenes?fechaIni=2017-04-01&fechaFin=2017-06-16&VIN=' + encodeURI(vin) + '&RFC=';
        var response = this.http.get(url).map(res => res.json());
        return response;
    }

}
