import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import{provideAnimations} from '@angular/platform-browser/animations'
import { NgxSpinnerModule } from 'ngx-spinner';


import { routes } from './app.routes';
import { HttpClient, provideHttpClient, withInterceptors,  } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { jwtInterceptor } from './_interceptors/jwt.interceptor';
import { loadingInterceptor } from './_interceptors/loading.interceptor';
import { TimeagoModule } from 'ngx-timeago';

export const appConfig: ApplicationConfig = {
  providers: [
    
    provideRouter(routes),
   provideHttpClient(withInterceptors([jwtInterceptor, loadingInterceptor])),
   provideAnimations() ,
   provideToastr({
    positionClass: 'toast-bottom-right'
   }),
  importProvidersFrom(NgxSpinnerModule, TimeagoModule.forRoot() )
  ]
};
