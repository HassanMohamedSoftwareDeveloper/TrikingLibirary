const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: []
});

export const state = initState;
export const getters = {
  trickById: state => id => state.tricks.find(x => x.slug === id),
  categoryById: state => id => state.categories.find(x => x.slug === id),
  difficultyById: state => id => state.difficulties.find(x => x.slug === id),
  trickItems: state => state.tricks.map(x => ({
    text: x.name,
    value: x.slug
  })),
  categoryItems: state => state.categories.map(x => ({
    text: x.name,
    value: x.slug
  })),
  difficultyItems: state => state.difficulties.map(x => ({
    text: x.name,
    value: x.slug
  })),
};

export const mutations = {
  setTricks(state, { tricks, categories, difficulties }) {
    state.tricks = tricks;
    state.categories = categories;
    state.difficulties = difficulties;
  },
  reset(state) {
    Object.assign(state, initState());
  }
};

export const actions = {
  async fetchTricks({ commit }) {
    try {
      const tricks = await this.$axios.$get("/api/trick");
      const categories = await this.$axios.$get("/api/categories");
      const difficulties = await this.$axios.$get("/api/difficulties");
      commit("setTricks", { tricks, categories, difficulties });

    } catch (error) {
      console.log(error);
    }
  },
  createTrick({ state, commit, dispatch }, { form }) {
    return this.$axios.$post("/api/trick", form);
  },
   updateTrick({ state, commit, dispatch }, { form }) {
    return this.$axios.$put("/api/trick", form);
  },
};
