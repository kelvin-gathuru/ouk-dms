import { ApplicationConfig, importProvidersFrom, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZoneChangeDetection, isDevMode, } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors, HTTP_INTERCEPTORS, withInterceptorsFromDi } from '@angular/common/http';
import { HttpRequestInterceptor } from './http-interceptor.module';
import { provideStore, } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideTranslateService } from '@ngx-translate/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { initializeApp } from '@core/security/initialize-app-factory';
import { SecurityService } from '@core/security/security.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { provideServiceWorker } from '@angular/service-worker';
import { ToastrService } from '@core/services/toastr-service';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { environment } from '@environments/environment'
import { provideAnimations } from '@angular/platform-browser/animations';

export function tokenGetter() {
  return localStorage.getItem('access_token');
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideTranslateService({
      loader: provideTranslateHttpLoader({ prefix: "/i18n/" }),
      fallbackLang: 'en',
      lang: 'en'
    }),
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(
      withInterceptors([HttpRequestInterceptor]), // <-- function interceptor
      withInterceptorsFromDi()                    // <-- also load DI interceptors
    ),
    JwtHelperService,
    provideAppInitializer(() =>
      initializeApp(inject(ToastrService), inject(SecurityService))()
    ),
    provideNativeDateAdapter(),
    provideStore({}),       // no root reducers needed for NgRx Data
    provideEffects([]),     // root effects (empty here)
    provideStoreDevtools({
      connectInZone: true,
      maxAge: 25
    }),
    importProvidersFrom(
      JwtModule.forRoot({
        config: {
          tokenGetter: tokenGetter,
          allowedDomains: [],
          disallowedRoutes: [],
        },
      }),
      MatSnackBarModule,
    )
    , provideServiceWorker('ngsw-worker.js', {
      enabled: environment.production,
      registrationStrategy: 'registerWhenStable:50000'
    }),
  ]
};
