import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ordenesws } from '../service/ordenesws';
import { ORDER_INFOPage } from '../ORDER-INFO/ORDER-INFO'

/*
  Generated class for the ORDER_LIST page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
    selector: 'page-ORDER_LIST',
    templateUrl: 'ORDER-LIST.html',
    providers: [ordenesws]
})
export class ORDER_LISTPage {

    ordenes: any;

    constructor(public navCtrl: NavController, public OrdenesWs: ordenesws) { }

    searchOrdenDB(event, key) {
        console.log(event.target.value);
        if (event.target.value.length > 3) {
            this.OrdenesWs.buscarOrden(event.target.value).subscribe(
                result => {
                    this.ordenes = result.ordenes;
                    console.log("result: " + result);
                    console.log("data: " + result.data);
                },
                err => {
                    console.log(err);
                },
                () => console.log('Busqueda de ordenes completado...')
            );
        }
    }   

    itemTapped(event, orden ) {
        this.navCtrl.push(ORDER_INFOPage, {
            orden: orden
        });
    }


    //ionViewDidLoad() {
    //    console.log('ionViewDidLoad ORDER_LISTPage');
    //}

}
