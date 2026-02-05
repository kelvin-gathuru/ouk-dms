//Validation to check if retention period days and action are set correctly
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const retentionValidator: ValidatorFn = (
  control: AbstractControl,
): ValidationErrors | null => {
  const retentionPeriodInDays = control.get('retentionPeriodInDays')?.value ? parseInt(control.get('retentionPeriodInDays')?.value, 10) : null;
  const onExpiryAction = control.get('onExpiryAction')?.value ? parseInt(control.get('onExpiryAction')?.value.toString(), 10) : 0;
  if (retentionPeriodInDays) {
    if (retentionPeriodInDays >= 0 && onExpiryAction === 0) {
      return { retentionPeriodAndAction: true };
    }
  }
  return null
};
