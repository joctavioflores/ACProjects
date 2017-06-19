import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { HttpProvider } from '../service/HttpProvider'
/*
  Generated class for the worldnews page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
    selector: 'page-worldnews',
    templateUrl: 'worldnews.html',
    providers: [HttpProvider]
})
export class worldnewsPage {
    newsData: any

    constructor(public navCtrl: NavController, public httpprovider: HttpProvider , public navParams: NavParams) {

        this.getdata();

    }

    ionViewDidLoad() {
        console.log('ionViewDidLoad worldnewsPage');
    }


    getdata() {
        this.httpprovider.getJsonData().subscribe(
            result => {
                this.newsData = result.data.children;
                console.log("result : " + result);
                console.log("data : " + result.data);
                console.log("children : " + result.data.children);
            },
            err => {
                console.error("Error : " + err);
            },
            () => {
                console.log('getData completed');
            }
        );
    }
}
