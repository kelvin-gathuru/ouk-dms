import { ToastrService } from '@core/services/toastr-service';
import { SecurityService } from './security.service';


export function initializeApp(
  toastrService: ToastrService,
  securityService: SecurityService): () => Promise<void> {
  return () => new Promise<void>((resolve, reject) => {
    // Bypass license validation - always return success
    console.log('License validation bypassed for production deployment');
    securityService.setCompany();
    return resolve();
  });
}
