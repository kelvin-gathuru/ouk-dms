import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { RouterModule } from '@angular/router';
import { CommonDialogService } from '@core/common-dialog/common-dialog.service';
import { TranslateModule } from '@ngx-translate/core';
import { BaseComponent } from '../../base.component';
import { Client } from '@core/domain-classes/client';
import { ClientStore } from '../client-store';
import { PageHelpTextComponent } from '@shared/page-help-text/page-help-text.component';
import { HasClaimDirective } from '@shared/has-claim.directive';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-client-list',
  imports: [
    FormsModule,
    TranslateModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    PageHelpTextComponent,
    HasClaimDirective,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './client-list.component.html',
  styleUrl: './client-list.component.scss'
})
export class ClientListComponent extends BaseComponent implements OnInit {

  clients: Client[] = [];
  displayedColumns: string[] = ['action', 'companyName', 'contactPerson', 'email', 'phoneNumber'];

  public clientStore = inject(ClientStore);
  private commonDialogService = inject(CommonDialogService);

  ngOnInit(): void {
    this.getClients();
  }

  getClients() {
    this.clientStore.loadClients()
  }

  deleteClient(client: Client) {
    this.sub$.sink = this.commonDialogService
      .deleteConfirmtionDialog(`${this.translationService.getValue('ARE_YOU_SURE_YOU_WANT_TO_DELETE')} ${client.companyName}`)
      .subscribe((isTrue: boolean) => {
        if (isTrue) {
          this.clientStore.deleteClientById(client.id ?? '');
        }
      });
  }
}
