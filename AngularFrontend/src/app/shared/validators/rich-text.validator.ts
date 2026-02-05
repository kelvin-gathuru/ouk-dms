import { AbstractControl, ValidationErrors } from '@angular/forms';

export class RichTextRequired {
    static validate(control: AbstractControl): ValidationErrors | null {
        if (control.value == null) {
            return { required: true };
        }

        const text = control.value
            .replace(/<p><\/p>/gi, '')
            .replace(/<br\s*\/?>/gi, '')
            .replace(/&nbsp;/gi, '')
            .replace(/<[^>]+>/g, '')
            .trim();

        return text.length ? null : { required: true };
    }
}