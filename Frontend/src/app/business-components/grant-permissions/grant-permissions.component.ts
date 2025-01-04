import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../../backend/services/home.service';
import { HomeManagementService } from '../../../backend/services/home-management.service';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { LoadingService } from '../../../backend/services/loading.service';

@Component({
  selector: 'app-grant-permissions',
  templateUrl: './grant-permissions.component.html',
  styles: ``
})
export class GrantPermissionsComponent implements OnInit {
  homeId!: string;
  membersHomePermissions: { email: string, permissions: string[] }[] = [];
  members: { value: string, label: string }[] = [];
  selectedMemberPermissions: any = {};
  homePermissions: string[] = [];
  grantPermissionsForm!: FormGroup;

  readonly formField: any = {
    members: {
      name: "members",
      options: this.members,
      required: "Device type is required",
      class: "form-select",
    }
  };

  constructor(
    private homeService: HomeService,
    private homeManagementService: HomeManagementService,
    private fb: FormBuilder,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    this.homeManagementService.homeId$.subscribe(id => {
      if (id) {
        this.homeId = id;
      }
    });
    this.homePermissions = this.homeManagementService.validHomePermissions;
    this.initializeForm();
    this.fetchMembersOnInit(this.homeId);
  }

  initializeForm() {
    const formControls: { [key: string]: FormControl } = {};

    this.homePermissions.forEach(permission => {
      formControls[permission] = new FormControl(false);
    });

    this.grantPermissionsForm = this.fb.group({
      members: new FormControl(''),
      ...formControls
    });
  }

  fetchMembersOnInit(id: string) {
    this.loadingService.show();
    this.homeService.getMembers(id).subscribe({
      next: (response) => {
        this.loadingService.hide();
        let membersHomePermissions = response.data.map((member: any) => ({
          email: member.email,
          permissions: member.homePermissions,
        }));
        let responseItems = response.data.map((member: any) => ({
          value: member.email,
          label: member.email,
        }));
        this.membersHomePermissions = membersHomePermissions;
        this.members = responseItems;
        this.grantPermissionsForm.get(this.formField.members.name)?.setValue(this.members[0].value);
        this.formField.members.options = this.members;
        this.disableMembersPermissions(this.members[0].value);
      },
      error: (error: any) => {
        this.loadingService.hide();
        console.error("Error fetching members:", error);
      }
    });
  }

  onSubmit() {
    if (!this.grantPermissionsForm.valid) {
      return;
    }

    const selectedPermissions: string[] = [];

    this.homePermissions.forEach(permission => {
      const control = this.grantPermissionsForm.get(permission);
      if (control?.value && !control.disabled) {
        selectedPermissions.push(permission);
      }
    });

    const permissionsData = {
      MemberEmail: this.grantPermissionsForm.get('members')?.value,
      HomePermissions: selectedPermissions
    };

    this.homeService.grantPermissions(this.homeId, permissionsData).subscribe({
      next: (response) => {
        this.loadMembers(this.homeId, permissionsData.MemberEmail);
      },
      error: (error: any) => {
        console.error("Error granting permissions:", error);
      }
    });
  }

  onSelectChange = (selectedOption: string) => {
    this.homePermissions.forEach(permission => {
      this.grantPermissionsForm.get(permission)?.setValue(false);
      this.grantPermissionsForm.get(permission)?.enable();
    });

    this.grantPermissionsForm.get(this.formField.members.name)?.setValue(selectedOption);

    this.disableMembersPermissions(selectedOption);
  }

  disableMembersPermissions(selectedOption: string) {
    this.homePermissions.forEach(permission => {
      if (this.membersHomePermissions.find(member => member.email === selectedOption)?.permissions.includes(permission)) {
        this.grantPermissionsForm.get(permission)?.setValue(true);
        this.grantPermissionsForm.get(permission)?.disable();
      }
      else {
        this.grantPermissionsForm.get(permission)?.setValue(false);
        this.grantPermissionsForm.get(permission)?.enable();
      }
    });

    this.selectedMemberPermissions = {};
  }

  loadMembers(id:string, email: string) {
    this.homeService.getMembers(id).subscribe({
      next: (response) => {
        let membersHomePermissions = response.data.map((member: any) => ({
          email: member.email,
          permissions: member.homePermissions,
        }));
        let responseItems = response.data.map((member: any) => ({
          value: member.email,
          label: member.email,
        }));
        this.membersHomePermissions = membersHomePermissions;
        this.members = responseItems;
        this.formField.members.options = this.members;
        this.disableMembersPermissions(email);
      }
    });
  }

  camelCaseToSpaces = (text: string) => {
    let spacedStr = text.replace(/([a-z])([A-Z])/g, '$1 $2');
    return spacedStr.charAt(0).toUpperCase() + spacedStr.slice(1).toLowerCase();
  }
}