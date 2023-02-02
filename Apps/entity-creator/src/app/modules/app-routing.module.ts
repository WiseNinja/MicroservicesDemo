import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapEntitiesComponent } from '../components/map-entities/map-entities.component';
import { MapsComponent } from '../components/maps/maps.component';

const routes: Routes = [ 
  { path: '', redirectTo: '/mapentities', pathMatch: 'full' },
  { path: 'mapentities', component: MapEntitiesComponent},
  { path: 'maps', component: MapsComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
