<template>
  <div class= "profile">
    <q-layout
      ref="layout"
      view="lHh Lpr fff"
      :left-class="{'bg-grey-2': true}">

      <!-- Toolbar -->
      <q-toolbar slot="header">
        <q-btn v-if="isAuthenticated"
          flat
          @click="$refs.layout.toggleLeft()">
          <q-icon name="menu" />
        </q-btn>
        <q-toolbar-title>Farmingdale
          <div slot="subtitle">Senior Project</div>
        </q-toolbar-title>
        <q-btn
          flat
          @click="$refs.layout.toggleRight()">
          <q-icon name="account box" />
        </q-btn>
      </q-toolbar>
      
      <q-tabs slot="navigation" v-if="isAuthenticated">
        <q-route-tab alert slot="title" to="/createticket" exact :count="ticketCount" name="tab-1" icon="message" />
        <q-route-tab slot="title" name="tab-2" to="/sites" icon="fingerprint" />
        <q-route-tab alert slot="title" to="/profile" exact name="tab-3" icon="account_box" />
        <q-route-tab slot="title" to="/technicians" name="tab-4" icon="accessibility" />
        <q-route-tab slot="title" to="/settings" name="tab-5" icon="build" />
      </q-tabs>

      <!-- Left Panel -->
      <div slot="left" v-if="isAuthenticated">
        <!--
          Use <q-side-link> component
          instead of <q-item> for
          internal vue-router navigation
        -->
        <div class="row flex-center">
          <img src="~/assets/logo2.jpg" style="height: 105px; width: 300px">      
        </div>

        <q-list separator v-if="isAuthenticated">
          <q-collapsible :key="nav.label" v-for="nav in navPoints" :group="nav.group" :icon="nav.icon" :label="nav.label">
            <q-side-link item :key="link.text" v-for="link in nav.links" link :to="link.url" exact>
              <q-item-side :icon="link.icon" />
              <q-item-main :label="link.text" />
            </q-side-link>
          </q-collapsible>
        </q-list>
      </div>

      <div slot="right">
        <q-list v-if="!isAuthenticated" no-border link inset-delimiter>
          <q-list-header>Account</q-list-header>
          <q-item @click="loginModal=true">
            <q-item-side icon="school" />
            <q-item-main label="Login" sublabel="Login or Register Account" />
          </q-item>
        </q-list>
        <q-list v-if="isAuthenticated" no-border link inset-delimiter>
          <q-list-header>Account</q-list-header>
          <q-item @click="signOut">
            <q-item-side icon="school" />
            <q-item-main label="Logout" sublabel="Log Out of Account" />
          </q-item>
        </q-list>
      </div>

      <q-modal :content-css="{minWidth: '25vw', minHeight: '400px', padding: '55px'}" v-model="loginModal" >
        <div v-if="userLogin === true" class="wrapper-login justify-center">
          <p class="caption">Sign In</p>
          <q-field>
            <q-input v-model="email" type="email" float-label="Email" />
          </q-field>
          <q-field>
            <q-input v-model="password" type="password" float-label="Password" />
          </q-field>
          <q-field>
            <div class="row justify-center">
              <q-btn color="primary text-center" @click="signInWithEmailAndPassword()">Login</q-btn>
            </div>
          </q-field>
          <q-field>
            <div class="row justify-center" @click="userLogin = false, userReg = true">
              <a>Register</a>
            </div>
            <div class="row justify-center"> 
              <a>Forgot or Reset Password</a>
            </div>
          </q-field>
        </div>
        <div v-if="userReg" class="wrapper-register justify-center">
          <p class="caption">Register</p>
          <q-field>
            <q-input v-model="form.firstname" type="email" float-label="First Name" />
          </q-field>
          <q-field>
            <q-input v-model="form.lastname" type="email" float-label="Last Name" />
          </q-field>
          <q-field>
            <q-input v-model="form.email" type="email" float-label="Email" />
          </q-field>
          <q-field>
            <q-input v-model="form.password" type="password" float-label="Password" />
          </q-field>
          <div class="row justify-center">
            <q-btn color="primary text-center" @click="createUserWithEmailAndPassword()">Register</q-btn>
          </div>
          <div @click="userLogin=true" class="row justify-center">
            <a href="https://www.w3schools.com">Login</a>
          </div>
          <div class="row justify-center"> 
            <a href="https://www.w3schools.com">Forgot or Reset Password</a>
          </div>
        </div>
      </q-modal>

      <ul class="breadcrumb">
        <li>
          <router-link to="/addsites">
            <q-icon name="home"/>Home
          </router-link>
        </li>
        <li>
          <router-link to="/sites">
            <q-icon name="home"/>Sites
          </router-link>
        </li>
        <li>
          <router-link to="/sites">
            <q-icon name="home"/>Tickets
          </router-link>
        </li>
        <li>
          <router-link to="/sites">
            <q-icon name="home"/>Machines
          </router-link>
        </li>
        <li>
          <router-link to="/sites">
            <q-icon name="home"/>Profile
          </router-link>
        </li>
        <li>
          <router-link to="/sites">
            <q-icon name="home"/>Home
          </router-link>
        </li>
      </ul>

      <router-view />

    </q-layout>
  </div>
</template>

<script>
import * as firebase from 'firebase'
import {
  openURL,
  QField,
  QLayout,
  QToolbar,
  QToolbarTitle,
  QBtn,
  QInput,
  QIcon,
  QList,
  QListHeader,
  QItem,
  QItemSide,
  QItemMain,
  QCollapsible,
  QCard,
  QCardTitle,
  QPopover,
  QCarousel,
  QTabs,
  QTab,
  QTabPane,
  QTransition,
  Alert,
  QModal,
  QRouteTab,
  QSideLink
} from 'quasar'

export default {
  name: 'layout',
  components: {
    openURL,
    QField,
    QLayout,
    QToolbar,
    QToolbarTitle,
    QBtn,
    QIcon,
    QInput,
    QList,
    QListHeader,
    QItem,
    QItemSide,
    QItemMain,
    QCollapsible,
    QCard,
    QCardTitle,
    QPopover,
    QCarousel,
    QTabs,
    QTab,
    QTabPane,
    QTransition,
    Alert,
    QModal,
    QRouteTab,
    QSideLink
  },
  data () {
    return {
      loginModal: false,
      userLogin: true,
      userReg: false,
      isAuthenticated: false,
      ticketCount: 0,
      user: null,
      form: {
        firstname: '',
        lastname: '',
        email: '',
        password: ''
      },
      email: '',
      password: '',
      navPoints: [{
        group: 'Technician',
        icon: 'supervisor_account',
        label: 'Technicians',
        links: {
          link1: {
            icon: 'person',
            text: 'View Technicians',
            url: '/technicians'
          },
          link2: {
            icon: 'person_add',
            text: 'Add Technician',
            url: '/addtechnicians'
          },
          link3: {
            icon: 'do_not_disturb_on',
            text: 'Remove Technician',
            url: '/profile'
          }
        }
      }, {
        group: 'Technician',
        icon: 'location_on',
        label: 'Sites',
        links: {
          link1: {
            icon: 'person',
            text: 'View Sites',
            url: '/sites'
          },
          link2: {
            icon: 'person_add',
            text: 'Add Site',
            url: '/siteinfo'
          }
        }
      }, {
        group: 'Technician',
        icon: 'desktop_windows',
        label: 'Machines',
        links: {
          link1: {
            icon: 'person',
            text: 'Machines',
            url: '/sites'
          },
          link2: {
            icon: 'person_add',
            text: 'Add Machine',
            url: '/siteinfo'
          }
        }
      }, {
        group: 'Technician',
        icon: 'assignment',
        label: 'Tickets',
        links: {
          link1: {
            icon: 'person',
            text: 'View Current Tickets',
            url: '/technicians'
          },
          link2: {
            icon: 'person_add',
            text: 'View Completed Tickets',
            url: '/addtechnicians'
          }
        }
      }]
    }
  },
  computed: {
  },
  methods: {
    launch (url) {
      openURL(url)
    },
    signOut () {
      firebase.auth().signOut().then(response => {
        this.$router.replace('/')
        this.isAuthenticated = false
      }).catch(error => {
        console.log(error)
      })
    },
    writeUserData (userId, firstname, lastname, email) {
      firebase.database().ref('users/' + userId).set({
        firstname: firstname,
        lastname: lastname,
        email: email
      })
    },
    createUserWithEmailAndPassword () {
      firebase.auth().createUserWithEmailAndPassword(this.email, this.password).catch(function (error) {
        var errorCode = error.code
        var errorMessage = error.message
        console.log(errorCode)
        console.log(errorMessage)
      }).then(user => {
        this.writeUserData(user.uid, this.form.firstname, this.form.lastname, user.email)
      })
    },
    signInWithEmailAndPassword () {
      firebase.auth().signInWithEmailAndPassword(this.email, this.password).catch(function (error) {
        var errorCode = error.code
        var errorMessage = error.message
        console.log(errorCode)
        console.log(errorMessage)
      }).then(user => {
        this.loginModal = false
        this.isAuthenticated = true
        this.$router.replace('profile')
      /*  if (this.user) {
          var userId = firebase.auth().currentUser.uid
          return firebase.database().ref('/users/' + userId).once('value').then(snapshot => {
            this.firstname = (snapshot.val() && snapshot.val().firstname)
            this.lastname = (snapshot.val() && snapshot.val().lastname)
          })
        } */
      })
    }
  },
  mounted () {
    var user = firebase.auth().currentUser
    this.user = user
    console.log(user)
    if (user != null) {
      this.isAuthenticated = true
      this.photoUrl = user.photoUrl
    }
  }
}
</script>

<style lang="stylus">
.card-wrapper {
  padding: 2%;
}
.q-carousel-track > div {
  min-height: 100px;
}
.card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
}
.card:hover {
  box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
.caption {
  font-size: 40px;
}
</style>