import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './modules/app-routing.module';
import { AppComponent } from './components/app/app.component';
import { AngularMaterialModule } from './modules/angular-material.module';
import { MapEntitiesComponent } from './components/map-entities/map-entities.component';
import { MapsComponent } from './components/maps/maps.component';
import { AngularSplitModule } from 'angular-split';
import { MapUploadDialog } from './components/maps/map-upload-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { StarRatingModule } from 'angular-star-rating';

@NgModule({
    declarations: [
        AppComponent,
        MapEntitiesComponent,
        MapsComponent,
        MapUploadDialog,
    ],
    providers: [],
    bootstrap: [AppComponent],
    imports: [
        BrowserModule,
        AppRoutingModule,
        AngularMaterialModule,
        BrowserAnimationsModule,
        AngularSplitModule,
        HttpClientModule,
        StarRatingModule.forRoot()
    ]
})
export class AppModule { }
