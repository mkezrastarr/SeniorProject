<template>
  <div>
    <q-card inline>
      <q-card-media>
        <img src="~assets/conklin.jpg"/>
        <q-card-title slot="overlay">
          Farmingdale Help Desk
          <span slot="subtitle">Subtitle</span>
        </q-card-title>
      </q-card-media>
      <q-card-main>
        {{message}}
      </q-card-main>
    </q-card>
  </div>
</template>

<script>
import {
  Alert,
  QField,
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
  QPopover,
  QInput,
  QCard,
  QCardTitle,
  QCardActions,
  QCardMedia,
  QRating,
  QCardMain,
  QCardSeparator,
  QModal,
  QCarousel
} from 'quasar'

export default {
  name: 'welcome',
  components: {
    Alert,
    QField,
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
    QPopover,
    QInput,
    QCard,
    QCardTitle,
    QCardActions,
    QCardMedia,
    QRating,
    QCardMain,
    QCardSeparator,
    QModal,
    QCarousel
  },
  data () {
    return {
      message: 'welcome!'
    }
  }
}
</script>

<style lang="stylus">
.q-toolbar {
  background-color: #007471;
}
.wrapper {
  margin:5%
}
.caption {
  font-size: 40px;
}
</style>
