import { inject, Injectable } from '@angular/core';
import { OnlineUser } from '@core/domain-classes/online-user';
import { SecurityService } from '@core/security/security.service';
import { environment } from '@environments/environment';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { DocumentStore } from '../../document/document-list/document-store';
import { FoldersViewStore } from '../../document/folders-view/folders-view-store';
import { AssignFoldersViewStore } from '../../document-library/folders-view/assign-folders-view-store';
import { ArchiveFoldersViewStore } from '../../archive-folders/archive-folders-view-store';
import { ArchiveDocumentsStore } from '../../archive-documents/archive-documents-store';
import { AiResponseMsg } from '@core/domain-classes/ai-response-msg';
import { CategoryService } from './category.service';

@Injectable({ providedIn: 'root' })
export class SignalrService {
  private hubConnection: signalR.HubConnection

  private _userNotification$: BehaviorSubject<string> = new BehaviorSubject<string>('');

  private _workItemNotification$: Subject<void> = new Subject<void>();

  private _refreshWorkflowSettings$: Subject<void> = new Subject<void>();

  private _aiPromptResponse$: Subject<AiResponseMsg> = new Subject<AiResponseMsg>();

  public get refreshWorkflowSettings$(): Observable<void> {
    return this._refreshWorkflowSettings$.asObservable();
  }

  public get workItemNotification$(): Observable<void> {
    return this._workItemNotification$.asObservable();
  }

  public get connectionId(): string | null {
    return this.hubConnection?.connectionId;
  }

  public get userNotification$(): Observable<string> {
    return this._userNotification$.asObservable();
  }
  public get aiPromptResponse$(): Observable<AiResponseMsg> {
    return this._aiPromptResponse$.asObservable();
  }
  documentStore = inject(DocumentStore);
  foldersViewStore = inject(FoldersViewStore);
  assignFoldersViewStore = inject(AssignFoldersViewStore);
  archiveFoldersViewStore = inject(ArchiveFoldersViewStore);
  archiveDocumentsStore = inject(ArchiveDocumentsStore);

  constructor(
    private categoryService: CategoryService,
    private securityService: SecurityService) { }

  public startConnection(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      const host = location.host;
      const protocal = location.protocol;
      const url = environment.apiUrl === '/' ? `${protocal}//${host}/` : environment.apiUrl;

      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${url}userHub`)
        .build();
      this.hubConnection
        .start()
        .then(() => {
          this.securityService.parseSecurityObj();
          resolve(true)
        })
        .catch(err => {
          reject(false);
        });
    });
  }

  addUser(signalrUser: OnlineUser) {
    signalrUser.connectionId = this.hubConnection?.connectionId ?? '';
    this.hubConnection?.invoke('join', signalrUser)
      .catch(err => {
        console.error(err)
      });
  }


  handleMessage = () => {
    this.hubConnection.on('userLeft', (id: string) => {
    });

    this.hubConnection.on('sendNotification', (userId: string) => {
      this._userNotification$.next(userId);
    });

    this.hubConnection.on('refreshDocuments', (categoryId: string) => {
      this.documentStore.loadDocuments();
      if (this.foldersViewStore.selectedCategoryId() == categoryId || (!this.foldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.foldersViewStore.loadCategoriesById(this.foldersViewStore.selectedCategoryId());
        this.foldersViewStore.loadDocuments();
      }
      if (this.assignFoldersViewStore.selectedCategoryId() == categoryId || (!this.assignFoldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.assignFoldersViewStore.loadCategoriesById(this.assignFoldersViewStore.selectedCategoryId());
        this.assignFoldersViewStore.loadDocuments();
      }
    });
    this.hubConnection.on('notifyUserPermissionChange', () => {
      this.foldersViewStore.loadDocuments();
    });

    this.hubConnection.on('refreshWorkflows', () => {
      this._workItemNotification$.next();
    });
    this.hubConnection.on('refreshWorkflowSettings', () => {
      this._refreshWorkflowSettings$.next();
    });
    this.hubConnection.on('updateUserPermission', (userId: string) => {
      this.securityService.refreshToken().subscribe(() => {
      });
    });

    this.hubConnection.on('sendAiPromptResponse', (msgId: string, msg: string) => {
      console.log('msgId', msgId);
      console.log('msg', msg);
      this._aiPromptResponse$.next({ msgId: msgId, msg: msg });
    });
    this.hubConnection.on('addEditFolder', () => {
      this.categoryService.getAllCategories(true).subscribe(() => {
      });
    });

    this.hubConnection.on('sharedFolder', (categoryId: string) => {
      if (this.foldersViewStore.selectedCategoryId() == categoryId || (!this.foldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.foldersViewStore.loadCategoriesById(this.foldersViewStore.selectedCategoryId());
        this.foldersViewStore.loadDocuments();
      }
      this.assignFoldersViewStore.loadCategoriesById(this.assignFoldersViewStore.selectedCategoryId());
      this.assignFoldersViewStore.loadDocuments();
    });

    this.hubConnection.on('sendNotificationFolderChange', (categoryId: string) => {
      if (this.foldersViewStore.selectedCategoryId() == categoryId || (!this.foldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.foldersViewStore.loadCategoriesById(this.foldersViewStore.selectedCategoryId());
        this.foldersViewStore.loadDocuments();
      }
      if (this.assignFoldersViewStore.selectedCategoryId() == categoryId || (!this.assignFoldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.assignFoldersViewStore.loadCategoriesById(this.assignFoldersViewStore.selectedCategoryId());
        this.assignFoldersViewStore.loadDocuments();
      }
    });
    this.hubConnection.on('archieveRestoreFolder', (categoryId: string) => {
      if (this.foldersViewStore.selectedCategoryId() == categoryId || (!this.foldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.foldersViewStore.loadCategoriesById(this.foldersViewStore.selectedCategoryId());
        this.foldersViewStore.loadDocuments();
      }
      if (this.assignFoldersViewStore.selectedCategoryId() == categoryId || (!this.assignFoldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.assignFoldersViewStore.loadCategoriesById(this.assignFoldersViewStore.selectedCategoryId());
        this.assignFoldersViewStore.loadDocuments();
      }
      if (this.archiveFoldersViewStore.selectedCategoryId() == categoryId || (!this.archiveFoldersViewStore.selectedCategoryId() && categoryId == null)) {
        this.archiveFoldersViewStore.loadCategoriesById(this.archiveFoldersViewStore.selectedCategoryId());
        this.archiveFoldersViewStore.loadDocuments();
      }
      this.archiveDocumentsStore.getArchiveDocuments();
    });

  }

}
