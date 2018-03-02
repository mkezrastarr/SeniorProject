<template>
  <div>
    <q-transition
      appear
      enter="fadeIn"
      leave="fadeOut">
      <div class="row justify-center"> 
        <q-card :key="site.siteid" v-for="site in sites" class="card" inline style="width: 300px">
          <a href="google.com">
            <div class="q-card-media relative-position">
              <img src="~/assets/header.jpg">
            </div>
          </a>
          <div class="q-card-primary q-card-container row no-wrap">
            <div class="col column">
              <div class="q-card-title">
              {{site.sitename}}
              </div>
              <div class="q-card-subtitle">
                <span>{{site.siteid}}</span>
              </div>
            </div>
          </div>
          <div class="col-auto self-center q-card-title-extra"></div>
        </q-card>
      </div>
    </q-transition>
  </div>
</template>

<script>
import 'quasar-extras/animate/fadeIn.css'
import 'quasar-extras/animate/fadeOut.css'
import {
  QLayout,
  QToolbar,
  QToolbarTitle,
  QBtn,
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
  QInput,
  QField
} from 'quasar'

export default {
  name: 'sites',
  components: {
    QLayout,
    QToolbar,
    QToolbarTitle,
    QBtn,
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
    QInput,
    QField
  },
  data () {
    return {
      sites: [],
      newsite: {
        sitename: '',
        longitude: 0,
        latitude: 0
      }
    }
  },
  computed: {
  },
  mounted () {
    this.GetSites()
  },
  methods: {
    GetSites () {
      fetch('api/systeminfo/sites', {
        headers: {
          'Accept': 'application/json',
          'cache-control': 'no-cache'
        },
        credentials: 'same-origin',
        method: 'GET'
      }).then(response => {
        return response.json()
      }).then(json => {
        this.sites = json
      })
    },
    AddSite () {
      fetch('/api/systeminfo/sites', {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.newsite)
      })
    }
  }
}
</script>

<style lang="stylus">
.card-wrapper {
  padding: 2%;
}
.card {
    box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
    transition: 0.3s;
}
.card:hover {
    box-shadow: 0 10px 25px 0 rgba(0,0,0,0.2);
}
</style>